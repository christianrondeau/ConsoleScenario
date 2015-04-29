using System;

namespace ConsoleScenario.Assertions
{
	public interface IScenarioStep
	{
		TimeSpan Timeout { get; }
		IInput Input { get; }
		IAssertion Assertion { get; }

		IScenarioStep WithTimeout(TimeSpan timeout);
	}

	public interface IInput
	{
		string Value { get; }
	}

	public interface IAssertion
	{
		AssertionResult Assert(int lineIndex, string actualLine);
	}

	public class ScenarioStep : IScenarioStep
	{
		public TimeSpan Timeout { get; private set; }
		public IInput Input { get; private set; }
		public IAssertion Assertion { get; private set; }

		private ScenarioStep()
		{
			Timeout = ConsoleScenarioDefaults.Timeout;
		}

		public ScenarioStep(IInput input) : this()
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
	}
}