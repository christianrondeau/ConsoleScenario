namespace ConsoleScenario.Assertions
{
	public class InputLineAssertion : IInput
	{
		public string Value { get; private set; }

		public InputLineAssertion(string value)
		{
			Value = value;
		}
	}
}