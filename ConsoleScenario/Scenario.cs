using System;
using System.Collections.Generic;

namespace ConsoleScenario
{
	public interface IScenario
	{
		void Run(TimeSpan? waitForExit = null);
		void AddStep(IScenarioStep step);
		void AddSteps(IEnumerable<IScenarioStep> steps);
	}

	public class Scenario : IScenario
	{
		private readonly List<IScenarioStep> _steps;
		private readonly IProcessFactory _processFactory;
		private readonly IAsyncDuplexStreamHandlerFactory _asyncDuplexStreamHandlerFactory;

		public Scenario(IProcessFactory processFactory, IAsyncDuplexStreamHandlerFactory asyncDuplexStreamHandlerFactory)
		{
			if (processFactory == null) throw new ArgumentNullException("processFactory");
			if (asyncDuplexStreamHandlerFactory == null) throw new ArgumentNullException("asyncDuplexStreamHandlerFactory");

			_processFactory = processFactory;
			_asyncDuplexStreamHandlerFactory = asyncDuplexStreamHandlerFactory;

			_steps = new List<IScenarioStep>();
		}

		public void AddStep(IScenarioStep step)
		{
			_steps.Add(step);
		}

		public void AddSteps(IEnumerable<IScenarioStep> steps)
		{
			_steps.AddRange(steps);
		}

		public void Run(TimeSpan? waitForExit = null)
		{
			using (var process = _processFactory.Start())
			{
				var lineIndex = -1;
				Exception scenarioAssertionException = null;
				try
				{
					var asyncTwoWayStreamsHandler = _asyncDuplexStreamHandlerFactory.Create(
											process.StandardOutput,
											process.StandardInput
											);

					Run(asyncTwoWayStreamsHandler, ref lineIndex);
				}
				catch (ScenarioAssertionException exc)
				{
					scenarioAssertionException = exc;
					throw;
				}
				finally
				{
					if (process.ForceExit(waitForExit) && scenarioAssertionException == null)
					{
						var lineThatNeverCameIndex = lineIndex + 1;
						throw new ScenarioAssertionException("Process wait for exit timeout", lineThatNeverCameIndex, null, null);
					}
				}
			}
		}

		private void Run(IAsyncDuplexStreamHandler asyncTwoWayStreamsHandler, ref int lineIndex)
		{
			var stepsEnumerator = _steps.GetEnumerator();

			while (stepsEnumerator.MoveNext())
			{
				var step = stepsEnumerator.Current;
				if (step == null) continue;

				var input = step.Input;
				if (input != null)
				{
					asyncTwoWayStreamsHandler.WriteLine(input);
					continue;
				}

				string actualLine;
				var repeat = step.Repeat;
				var assertion = step.Assertion;

				if (assertion == null)
					throw new NotSupportedException("Only IInput and IAssertion are supported");

				do
				{
					lineIndex++;
					actualLine = ReadLineOrTimeout(lineIndex, asyncTwoWayStreamsHandler, step.Timeout);
					var assertionResult = assertion.Assert(actualLine);

					if (!assertionResult.Success)
						throw new ScenarioAssertionException(assertionResult.Message, lineIndex, actualLine, assertionResult.Expected);
				} while (--repeat > 0 && actualLine != null);
			}
		}

		private static string ReadLineOrTimeout(int lineIndex, IAsyncDuplexStreamHandler asyncTwoWayStreamsHandler, TimeSpan timeout)
		{
			try
			{
				return asyncTwoWayStreamsHandler.ReadLine(timeout.TotalSeconds);
			}
			catch (TimeoutException exc)
			{
				throw new ScenarioAssertionException("Timeout", lineIndex, null, null, exc);
			}
		}
	}
}