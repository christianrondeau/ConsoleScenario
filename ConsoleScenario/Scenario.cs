using System;
using System.Diagnostics;

namespace ConsoleScenario
{
	public interface IScenario
	{
		IScenario Expect(string output);
		void Run();
	}

	public class Scenario : IScenario
	{
		private readonly Process _process;
		private string _expectedOutput;
		private readonly IAsyncDuplexStreamHandlerFactory _asyncDuplexStreamHandlerFactory;

		public Scenario(Process process, IAsyncDuplexStreamHandlerFactory asyncDuplexStreamHandlerFactory)
		{
			if (process == null) throw new ArgumentNullException("process");
			if (asyncDuplexStreamHandlerFactory == null) throw new ArgumentNullException("asyncDuplexStreamHandlerFactory");

			_process = process;
			_asyncDuplexStreamHandlerFactory = asyncDuplexStreamHandlerFactory;
		}

		public IScenario Expect(string output)
		{
			_expectedOutput = output;
			return this;
		}

		public void Run()
		{
			_process.Start();

			var asyncTwoWayStreamsHandler = _asyncDuplexStreamHandlerFactory.Create(_process.StandardOutput, _process.StandardInput);

			var output = asyncTwoWayStreamsHandler.ReadUntil(10, "");

			if (output != _expectedOutput)
				throw new ScenarioAssertionException("Invalid console output", output, _expectedOutput);
		}
	}
}