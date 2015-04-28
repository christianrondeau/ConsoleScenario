using System;
using System.Collections.Generic;
using ConsoleScenario.Assertions;

namespace ConsoleScenario
{
	public interface IScenario
	{
		void Run(TimeSpan? waitForExit = null);
		void AddAssertion(IScenarioStep assertion);
		void AddAssertions(IEnumerable<IScenarioStep> assertions);
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

		public void AddAssertion(IScenarioStep step)
		{
			_steps.Add(step);
		}

		public void AddAssertions(IEnumerable<IScenarioStep> steps)
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

				var input = step as IInput;
				if (input != null)
				{
					asyncTwoWayStreamsHandler.WriteLine(input.Value);
					continue;
				}

				string actualLine;
				var assertion = step as IAssertion;
				AssertionResult assertionResult;

				if (assertion == null)
					throw new NotSupportedException("Only IInput and IAssertion are supported");

				do
				{
					lineIndex++;
					actualLine = ReadLineOrTimeout(lineIndex, asyncTwoWayStreamsHandler, assertion.Timeout);
					assertionResult = assertion.Assert(lineIndex, actualLine);
				} while (assertionResult == AssertionResult.KeepUsingSameAssertion && actualLine != null);
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