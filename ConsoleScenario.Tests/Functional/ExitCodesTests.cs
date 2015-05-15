using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Functional
{
	public class ExitCodesTests : EndToEndTestsBase
	{
		[Test]
		public void SuccessWithExpectedResultCode()
		{
			GivenATestConsoleScenario("exit-code")
				.ExpectExitCode(-1)
				.Run();
		}

		[Test]
		public void FailureWithUnexpectedError()
		{
			ScenarioHelper.Do(() =>
				GivenATestConsoleScenario("exit-code")
					.Run(),
				ScenarioHelper.Expect(
					"Unexpected exit code",
					1,
					"-1",
					"0"
					));
		}
	}
}