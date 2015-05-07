using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Functional
{
	public class UntilTests : EndToEndTestsBase
	{
		private const string TestName = "count-to-ten";

		[Test]
		public void SuccessWithUntilAndExpect()
		{
			GivenATestConsoleScenario(TestName)
				.Until(line => line == "9")
				.Expect("10")
				.Run();
		}

		[Test]
		public void FailureBecauseUntilNeverComes()
		{
			ScenarioHelper.Do(() =>
				GivenATestConsoleScenario(TestName)
					.Until(line => line == "this will never be true")
					.Run(),
				ScenarioHelper.Expect(
					"Until was never true",
					11,
					null,
					(string)null
					));
		}
	}
}