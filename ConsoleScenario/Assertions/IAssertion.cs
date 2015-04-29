namespace ConsoleScenario.Assertions
{
	public interface IAssertion
	{
		AssertionResult Assert(string actualLine);
	}
}