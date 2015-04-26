using System;

namespace ConsoleScenario.Assertions
{
	public class NoExtraneousLinesAssertion : AssertionBase, IAssertion
	{
		public NoExtraneousLinesAssertion()
		{
		}

		public NoExtraneousLinesAssertion(TimeSpan timeout)
			: base(timeout)
		{
		}

		public AssertionResult Assert(int lineIndex, string actualLine)
		{
			if (actualLine != null)
				throw new ScenarioAssertionException("Extraneous line", lineIndex, actualLine, null);

			return AssertionResult.KeepUsingSameAssertion;
		}
	}
}