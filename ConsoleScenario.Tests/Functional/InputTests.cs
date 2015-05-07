using NUnit.Framework;

namespace ConsoleScenario.Tests.Functional
{
	public class InputTests : EndToEndTestsBase
	{
		[Test]
		public void SuccessWithInputValue()
		{
			GivenATestConsoleScenario("print-input")
				.Expect("Enter a value:")
				.Input("my text")
				.Expect("You have entered: my text")
				.Run();
		}
	}
}