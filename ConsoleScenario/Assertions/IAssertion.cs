using System;

namespace ConsoleScenario.Assertions
{
	public interface IAssertion
	{
		AssertionResult Assert(int lineIndex, string actualLine);
		TimeSpan Timeout { get; }
	}
}