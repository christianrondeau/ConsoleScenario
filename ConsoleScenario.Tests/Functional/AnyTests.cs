using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Functional
{
	public class AnyTests : EndToEndTestsBase
	{
		private const string TestName = "three-lines";

		[Test]
		public void SuccessWithSingleAny()
		{
			GivenATestConsoleScenario(TestName)
				.Any()
				.Expect("This is the middle line.")
				.Any()
				.Run();
		}

		[Test]
		public void SuccessWithMultipleAny()
		{
			GivenATestConsoleScenario(TestName)
				.Any(3)
				.Run();
		}

		[Test]
		public void FailureBecauseAnyLineNeverComes()
		{
			ScenarioHelper.Do(() =>
				GivenATestConsoleScenario(TestName)
					.Any(4)
					.Run(),
				ScenarioHelper.Expect(
					"Missing line",
					4,
					null,
					(string)null
					));
		}
	}
}