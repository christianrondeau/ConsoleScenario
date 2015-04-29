namespace ConsoleScenario.Assertions
{
	public class NoExtraneousLinesAssertion : IAssertion
	{
		public AssertionResult Assert(string actualLine)
		{
			return actualLine != null
				? AssertionResult.Fail("Extraneous line")
				: AssertionResult.Pass();
		}
	}
}