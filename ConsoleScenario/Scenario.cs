using System;
using System.Collections.Generic;
using System.Diagnostics;
using ConsoleScenario.Assertions;

namespace ConsoleScenario
{
	public interface IScenario
	{
		void Run();
		void AddAssertion(IAssertion assertion);
		void AddAssertions(IEnumerable<IAssertion> assertions);
	}

	public class Scenario : IScenario
	{
		private const double ReadLineTimeoutInSeconds = 0.5d;

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

		public void Run()
		{
			using (_process)
			{
				_process.Start();

				var asyncTwoWayStreamsHandler = _asyncDuplexStreamHandlerFactory.Create(_process.StandardOutput,
					_process.StandardInput);

				var lineIndex = -1;
				var assertionsEnumerator = _lineAssertions.GetEnumerator();

				while(assertionsEnumerator.MoveNext())
				{
					var assertion = assertionsEnumerator.Current;
					if (assertion == null) break;
					string actualLine;

					do
					{
						lineIndex++;
						actualLine = asyncTwoWayStreamsHandler.ReadLine(ReadLineTimeoutInSeconds);
					} while (assertion.Assert(lineIndex, actualLine) == AssertionResult.KeepUsingSameAssertion && actualLine != null);
				}

				var extraneousLine = asyncTwoWayStreamsHandler.ReadLine(ReadLineTimeoutInSeconds);

				if (extraneousLine != null)
					throw new ScenarioAssertionException("Extraneous line", lineIndex + 1, extraneousLine, null);

				asyncTwoWayStreamsHandler.WaitForExit();
			}
		}
	}
}