using System.Text.RegularExpressions;
using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Functional
{
	public class CallbackTests : EndToEndTestsBase
	{
		[Test]
		public void SuccessWithCallbacks()
		{
			GivenATestConsoleScenario("three-lines")
				.Expect(line => line != null)
				.Expect(line => line == "This is the middle line.")
				.Expect(line => line.Contains("last"))
				.Run();
		}

		[Test]
		public void CanExtractValuesWithRegexHelper()
		{
			string guid = null;
			GivenATestConsoleScenario("print-guid")
				.Extract("The guid will be: (.+)", vars => guid = vars[0])
				.Expect(() => string.Format("The guid is: {0}", guid))
				.Run();
		}

		[Test]
		public void FailureBecauseCallbackReturnsFalse()
		{
			ScenarioHelper.Do(() =>
				GivenATestConsoleScenario("three-lines")
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
				GivenATestConsoleScenario("three-lines")
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