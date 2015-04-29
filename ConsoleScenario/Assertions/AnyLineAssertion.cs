namespace ConsoleScenario.Assertions
{
	public class AnyLineAssertion : IAssertion
	{
		public AssertionResult Assert(string actualLine)
		{
			return actualLine == null
				? AssertionResult.Fail("Missing line")
				: AssertionResult.Pass();
		}
	}
}