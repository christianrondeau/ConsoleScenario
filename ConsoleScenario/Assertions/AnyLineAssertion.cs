using System;

namespace ConsoleScenario.Assertions
{
	public class AnyLineAssertion : AssertionBase, IAssertion
	{
		public AnyLineAssertion()
		{
		}

		public AnyLineAssertion(TimeSpan timeout)
			: base(timeout)
		{
		}

		public AssertionResult Assert(int lineIndex, string actualLine)
		{
			return AssertionResult.MoveToNextAssertion;
		}
	}
}