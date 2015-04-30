namespace ConsoleScenario
{
	public interface IAssertion
	{
		AssertionResult Assert(string actualLine);
	}
}