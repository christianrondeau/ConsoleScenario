using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Functional
{
	public class OneLineTests : EndToEndTestsBase
	{
		private const string TestName = "one-line";

		[Test]
		public void Success()
		{
			GivenATestConsoleScenario(TestName)
				.Expect("Single line output.")
				.Run();
		}

		[Test]
		public void FailureBecauseTextIsDifferent()
		{
			ScenarioHelper.Do(() =>
				GivenATestConsoleScenario(TestName)
					.Expect("This is not what I expected...")
					.Run(),
				ScenarioHelper.Expect(
					"Unexpected line",
					1,
					"Single line output.",
					"This is not what I expected..."
					));
		}
	}
}