using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Functional
{
	public class ErrorTests : EndToEndTestsBase
	{
		private const string TestName = "error";
		
		[Test]
		public void SuccessWithExpectedError()
		{
			GivenATestConsoleScenario(TestName)
				.ExpectError("This is the error text")
				.IgnoreExitCode()
				.Run();
		}

		[Test]
		public void FailureWithUnexpectedError()
		{
			ScenarioHelper.Do(() =>
				GivenATestConsoleScenario(TestName)
					.Expect("Error incoming...")
					.Any()
					.IgnoreExitCode()
					.Run(),
				ScenarioHelper.Expect(
					"Unexpected console error",
					2,
					"Unhandled Exception: System.ApplicationException: This is the error text",
					(string)null
					));
		}
	}
}