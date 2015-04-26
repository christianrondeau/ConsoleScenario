using System;
using System.Collections.Generic;
using System.Diagnostics;
using ConsoleScenario.Assertions;

namespace ConsoleScenario
{
	public interface IScenario
	{
		void Run(TimeSpan? waitForExit = null);
		void AddAssertion(IAssertion assertion);
		void AddAssertions(IEnumerable<IAssertion> assertions);
	}

	public class Scenario : IScenario
	{
		private readonly Process _process;
		private readonly List<IAssertion> _lineAssertions;
		private readonly IAsyncDuplexStreamHandlerFactory _asyncDuplexStreamHandlerFactory;

		public Scenario(Process process, IAsyncDuplexStreamHandlerFactory asyncDuplexStreamHandlerFactory)
		{
			if (process == null) throw new ArgumentNullException("process");
			if (asyncDuplexStreamHandlerFactory == null) throw new ArgumentNullException("asyncDuplexStreamHandlerFactory");

			_process = process;
			_asyncDuplexStreamHandlerFactory = asyncDuplexStreamHandlerFactory;

			_lineAssertions = new List<IAssertion>();
		}

		public void AddAssertion(IAssertion assertion)
		{
			_lineAssertions.Add(assertion);
		}

		public void AddAssertions(IEnumerable<IAssertion> assertions)
		{
			_lineAssertions.AddRange(assertions);
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

					var assertionsEnumerator = _lineAssertions.GetEnumerator();

					while (assertionsEnumerator.MoveNext())
					{
						var assertion = assertionsEnumerator.Current;
						if (assertion == null) break;
						string actualLine;

						do
						{
							lineIndex++;
							actualLine = ReadLineOrTimeout(lineIndex, asyncTwoWayStreamsHandler, assertion.Timeout);
						} while (assertion.Assert(lineIndex, actualLine) == AssertionResult.KeepUsingSameAssertion && actualLine != null);
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