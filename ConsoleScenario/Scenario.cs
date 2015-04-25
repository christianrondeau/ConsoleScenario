using System;
using System.Collections.Generic;
using System.Diagnostics;

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
		private const int ReadLineTimeoutInSeconds = 10;

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

				for (var lineIndex = 0; lineIndex < _lineAssertions.Count; lineIndex++)
				{
					var actualLine = asyncTwoWayStreamsHandler.ReadLine(ReadLineTimeoutInSeconds);
					var assertion = _lineAssertions[lineIndex];

					assertion.Assert(lineIndex, actualLine);
				}

				asyncTwoWayStreamsHandler.WaitForExit();
			}
		}
	}
}