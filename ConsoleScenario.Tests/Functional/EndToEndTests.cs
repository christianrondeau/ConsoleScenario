using System;
using System.IO;
using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Functional
{
	public class EndToEndTests
	{
		public class OneLineTests
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

		public class MultiLinesTests
		{
			private const string TestName = "two-lines";

			[Test]
			public void Success()
			{
				GivenATestConsoleScenario(TestName)
					.Expect(
					"Line 1",
					"Line 2"
					)
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
						.Expect(
							"Line 1"
						)
						.Run(),
					ScenarioHelper.Expect(
						"Extraneous line",
						2,
						"Line 1",
						null
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
						2,
						null,
						"Line 3"
						));
			}
		}

		private static IScenario GivenATestConsoleScenario(string testName)
		{
			var appPath = Path.Combine(Path.GetDirectoryName(new Uri(typeof(EndToEndTests).Assembly.CodeBase).LocalPath),
				@"ConsoleScenario.TestApp.exe");

			return Scenarios.Create(appPath, testName);
		}
	}
}
