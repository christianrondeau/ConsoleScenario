using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Functional
{
	public class CallbackTests : EndToEndTestsBase
	{
		private const string TestName = "three-lines";

		[Test]
		public void SuccessWithCallbacks()
		{
			GivenATestConsoleScenario(TestName)
				.Expect(line => line != null)
				.Expect(line => line == "This is the middle line.")
				.Expect(line => line.Contains("last"))
				.Run();
		}

		[Test]
		public void FailureBecauseCallbackReturnsFalse()
		{
			ScenarioHelper.Do(() =>
				GivenATestConsoleScenario(TestName)
					.Expect(line => false)
					.Run(),
				ScenarioHelper.Expect(
					"Unexpected line",
					1,
					"This is the first line.",
					expected => expected.StartsWith("<FailureBecauseCallbackReturnsFalse>")
					));
		}

		[Test]
		public void FailureBecauseMissingLine()
		{
			ScenarioHelper.Do(() =>
				GivenATestConsoleScenario(TestName)
					.Any(3)
					.Expect(line => true)
					.Run(),
				ScenarioHelper.Expect(
					"Missing line",
					4,
					null,
					expected => expected.StartsWith("<FailureBecauseMissingLine>")
					));
		}
	}
}