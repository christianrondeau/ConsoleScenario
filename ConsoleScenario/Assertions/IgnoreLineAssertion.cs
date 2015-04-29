namespace ConsoleScenario.Assertions
{
	public class IgnoreLineAssertion : IAssertion
	{
		public AssertionResult Assert(string actualLine)
		{
			return AssertionResult.Pass();
		}
	}
}