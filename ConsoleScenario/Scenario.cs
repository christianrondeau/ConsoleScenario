using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		private readonly Process _process;
		private readonly List<IScenarioStep> _steps;
		private readonly IAsyncDuplexStreamHandlerFactory _asyncDuplexStreamHandlerFactory;

		public Scenario(Process process, IAsyncDuplexStreamHandlerFactory asyncDuplexStreamHandlerFactory)
		{
			if (process == null) throw new ArgumentNullException("process");
			if (asyncDuplexStreamHandlerFactory == null) throw new ArgumentNullException("asyncDuplexStreamHandlerFactory");

			_process = process;
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
			using (_process)
			{
				var lineIndex = -1;
				Exception scenarioAssertionException = null;
				try
				{
					_process.Start();

					var asyncTwoWayStreamsHandler = _asyncDuplexStreamHandlerFactory.Create(_process.StandardOutput,
						_process.StandardInput);

					var stepsEnumerator = _steps.GetEnumerator();

					while (stepsEnumerator.MoveNext())
					{
						var step = stepsEnumerator.Current;
						if (step == null) break;
						string actualLine = null;

						AssertionResult assertionResult;
						do
						{
							var input = step as IInput;
							if (input != null)
							{
								asyncTwoWayStreamsHandler.WriteLine(input.Value);
								assertionResult = AssertionResult.MoveToNextAssertion;
								continue;
							}

							var assertion = step as IAssertion;
							if (assertion != null)
							{

								lineIndex++;
								actualLine = ReadLineOrTimeout(lineIndex, asyncTwoWayStreamsHandler, assertion.Timeout);
								assertionResult = assertion.Assert(lineIndex, actualLine);
							}
							else
							{
								throw new NotSupportedException("Only IInput and IAssertion are supported");
							}
						} while (assertionResult == AssertionResult.KeepUsingSameAssertion && actualLine != null);
					}
				}
				catch (ScenarioAssertionException exc)
				{
					scenarioAssertionException = exc;
					throw;
				}
				finally
				{
					if (!_process.HasExited)
					{
						_process.CloseMainWindow();

						if (!_process.WaitForExit(
							waitForExit.HasValue
								? (int) waitForExit.Value.TotalMilliseconds
								: (int) ConsoleScenarioDefaults.Timeout.TotalSeconds))
						{
							_process.Kill();
							if (scenarioAssertionException == null)
							{
								var lineThatNeverCameIndex = lineIndex + 1;
								throw new ScenarioAssertionException("Process wait for exit timeout", lineThatNeverCameIndex, null, null);
							}
						}
					}
				}
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