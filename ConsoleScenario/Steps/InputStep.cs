using System;
using ConsoleScenario.StreamHandling;

namespace ConsoleScenario.Steps
{
	public class InputStep : IScenarioStep
	{
		public string Input { get; private set; }

		public InputStep(string input)
		{
			if (input == null) throw new ArgumentNullException("input");

			Input = input;
		}

		public void Run(IAsyncDuplexStreamHandler asyncDuplexStreamHandler, ref int lineIndex)
		{
			asyncDuplexStreamHandler.WriteLine(Input);
		}
	}
}