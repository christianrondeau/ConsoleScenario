using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Functional
{
	public class InputWithPromptTests : EndToEndTestsBase
	{
		[Test]
		public void SuccessWithInputValueInPromptWithText()
		{
			GivenATestConsoleScenario("yes-no")
				.ExpectPrompt("Do you want to continue? (y/n): ")
				.Input("y")
				.Expect("You have selected yes")
				.Run();
		}

		[Test]
		public void FailureBecauseEndOfStream()
		{
			ScenarioHelper.Do(() =>
				GivenATestConsoleScenario("one-line")
					.Any()
					.ExpectPrompt("Expected")
					.Run(),
				ScenarioHelper.Expect(
					"Unexpected end of stream",
					2,
					null,
					"Expected"
					));
		}

		[Test]
		public void FailureBecauseEndOfLine()
		{
			ScenarioHelper.Do(() =>
				GivenATestConsoleScenario("two-lines")
					.ExpectPrompt("Line 1 (from here, an unexpected line break is expected)")
					.Run(),
				ScenarioHelper.Expect(
					"Unexpected end of line",
					1,
					"Line 1",
					"Line 1 (from here, an unexpected line break is expected)"
					));
		}

		[Test]
		public void FailureBecauseUnexpectedPrompt()
		{
			ScenarioHelper.Do(() =>
				GivenATestConsoleScenario("one-line")
					.ExpectPrompt("Single line surprise!")
					.Run(),
				ScenarioHelper.Expect(
					"Unexpected prompt at character 12",
					1,
					"Single line ",
					"Single line surprise!"
					));
		}
	}
}