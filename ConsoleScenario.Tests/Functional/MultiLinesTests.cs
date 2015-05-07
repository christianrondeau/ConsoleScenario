using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Functional
{
	public class MultiLinesTests : EndToEndTestsBase
	{
		private const string TestName = "two-lines";

		[Test]
		public void SuccessWithMultilineExpect()
		{
			GivenATestConsoleScenario(TestName)
				.Expect(
					"Line 1",
					"Line 2"
				)
				.Run();
		}

		[Test]
		public void SuccessWithMultipleExpects()
		{
			GivenATestConsoleScenario(TestName)
				.Expect("Line 1")
				.Expect("Line 2")
				.Run();
		}

		[Test]
		public void SuccessWithIgnoreRemaining()
		{
			GivenATestConsoleScenario(TestName)
				.Expect("Line 1")
				.IgnoreRemaining()
				.Run();
		}

		[Test]
		public void FailureBecauseTextIsDifferent()
		{
			ScenarioHelper.Do(() =>
				GivenATestConsoleScenario(TestName)
					.Expect(
						"Line 1",
						"This one is wrong"
					)
					.Run(),
				ScenarioHelper.Expect(
					"Unexpected line",
					2,
					"Line 2",
					"This one is wrong"
					));
		}

		[Test]
		public void FailureBecauseOfAnExtraneousLine()
		{
			ScenarioHelper.Do(() =>
				GivenATestConsoleScenario(TestName)
					.Expect("Line 1")
					.ExpectNothingElse()
					.Run(),
				ScenarioHelper.Expect(
					"Extraneous line",
					2,
					"Line 2",
					(string)null
					));
		}

		[Test]
		public void FailureBecauseLineIsMissing()
		{
			ScenarioHelper.Do(() =>
				GivenATestConsoleScenario(TestName)
					.Expect(
						"Line 1",
						"Line 2",
						"Line 3"
					)
					.Run(),
				ScenarioHelper.Expect(
					"Missing line",
					3,
					null,
					"Line 3"
					));
		}
	}
}