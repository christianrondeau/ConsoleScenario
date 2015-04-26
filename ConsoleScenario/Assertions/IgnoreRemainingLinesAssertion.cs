using System;

namespace ConsoleScenario.Assertions
{
	public class IgnoreRemainingLinesAssertion : AssertionBase, IAssertion
	{
		public IgnoreRemainingLinesAssertion()
		{
		}

		public IgnoreRemainingLinesAssertion(TimeSpan timeout)
			: base(timeout)
		{
		}

		public AssertionResult Assert(int lineIndex, string actualLine)
		{
			return AssertionResult.KeepUsingSameAssertion;
		}
	}
}