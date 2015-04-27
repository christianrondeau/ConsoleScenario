using System;

namespace ConsoleScenario.Assertions
{
	public interface IScenarioStep
	{
	}

	public interface IInput : IScenarioStep
	{
		string Value { get; }
	}

	public interface IAssertion : IScenarioStep
	{
		TimeSpan Timeout { get; }
		AssertionResult Assert(int lineIndex, string actualLine);
	}
}