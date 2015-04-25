using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleScenario
{
	public interface IScenario
	{
		IScenario Expect(params string[] lines);
		void Run();
	}

	public class Scenario : IScenario
	{
		private const int ReadLineTimeoutInSeconds = 10;

		private readonly Process _process;
		private readonly List<string> _expectedLines;
		private readonly IAsyncDuplexStreamHandlerFactory _asyncDuplexStreamHandlerFactory;

		public Scenario(Process process, IAsyncDuplexStreamHandlerFactory asyncDuplexStreamHandlerFactory)
		{
			if (process == null) throw new ArgumentNullException("process");
			if (asyncDuplexStreamHandlerFactory == null) throw new ArgumentNullException("asyncDuplexStreamHandlerFactory");

			_process = process;
			_asyncDuplexStreamHandlerFactory = asyncDuplexStreamHandlerFactory;

			_expectedLines = new List<string>();
		}

		public IScenario Expect(params string[] lines)
		{
			_expectedLines.AddRange(lines);
			return this;
		}

		public void Run()
		{
			_process.Start();

			var asyncTwoWayStreamsHandler = _asyncDuplexStreamHandlerFactory.Create(_process.StandardOutput, _process.StandardInput);

			for (var lineIndex = 0; lineIndex < _expectedLines.Count; lineIndex++)
			{
				var actualLine = asyncTwoWayStreamsHandler.ReadLine(ReadLineTimeoutInSeconds);

				var expectedLine = _expectedLines[lineIndex];

				if (actualLine != expectedLine)
					throw new ScenarioAssertionException("Unexpected line", lineIndex, actualLine, expectedLine);
			}
		}
	}
}