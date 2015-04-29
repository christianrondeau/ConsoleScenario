namespace ConsoleScenario.Assertions
{
	public class IgnoreRemainingLinesAssertion : IAssertion
	{
		public AssertionResult Assert(int lineIndex, string actualLine)
		{
			return AssertionResult.KeepUsingSameAssertion;
		}
	}
}