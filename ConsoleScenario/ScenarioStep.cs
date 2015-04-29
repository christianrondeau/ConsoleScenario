using System;
using ConsoleScenario.Assertions;

namespace ConsoleScenario
{
	public interface IScenarioStep
	{
		TimeSpan Timeout { get; }
		string Input { get; }
		IAssertion Assertion { get; }
		int Repeat { get; }

		IScenarioStep WithTimeout(TimeSpan timeout);
		IScenarioStep Times(int times);
	}

	public class ScenarioStep : IScenarioStep
	{
		public TimeSpan Timeout { get; private set; }
		public string Input { get; private set; }
		public IAssertion Assertion { get; private set; }
		public int Repeat { get; private set; }

		private ScenarioStep()
		{
			Repeat = 1;
			Timeout = ConsoleScenarioDefaults.Timeout;
		}

		public ScenarioStep(string input)
			: this()
		{
			if (input == null) throw new ArgumentNullException("input");
			Input = input;
		}

		public ScenarioStep(IAssertion assertion) : this()
		{
			if (assertion == null) throw new ArgumentNullException("assertion");
			Assertion = assertion;
		}

		public IScenarioStep WithTimeout(TimeSpan timeout)
		{
			Timeout = timeout;
			return this;
		}

		public IScenarioStep Times(int times)
		{
			Repeat = times;
			return this;
		}
	}
}