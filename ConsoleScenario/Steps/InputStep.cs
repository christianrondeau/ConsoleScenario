using System;

namespace ConsoleScenario.Steps
{
	public interface IInputStep : IScenarioStep
	{
	}

	public class InputStep : IInputStep
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