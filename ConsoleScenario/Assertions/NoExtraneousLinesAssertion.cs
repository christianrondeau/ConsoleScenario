namespace ConsoleScenario.Assertions
{
	public class NoExtraneousLinesAssertion : IAssertion
	{
		public AssertionResult Assert(int lineIndex, string actualLine)
		{
			if (actualLine != null)
				throw new ScenarioAssertionException("Extraneous line", lineIndex, actualLine, null);

			return AssertionResult.KeepUsingSameAssertion;
		}
	}
}