using System;
using System.Diagnostics;
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
						3,
						null,
						"Line 3"
						));
			}
		}

		public class AnyTests
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
						null
						));
			}
		}

		public class InputTests
		{
			private const string TestName = "print-input";

			[Test]
			public void SuccessWithInputValue()
			{
				GivenATestConsoleScenario(TestName)
					.Expect("Enter a value:")
					.Input("my text")
					.Expect("You have entered: my text")
					.Run();
			}
		}

		public class TimeoutTests
		{
			private const string TestName = "timeout";

			[Test]
			public void FailureBecauseConsoleDidNotRespond()
			{

				ScenarioHelper.Do(() =>
					GivenATestConsoleScenario(TestName)
						.Expect("Waiting for 2 seconds...")
						.Expect("This line will never come.", TimeSpan.FromSeconds(0.5))
						.Run(),
					ScenarioHelper.Expect(
						"Timeout",
						2,
						null,
						null
						));
			}

			[Test]
			public void FailureAndKillConsoleWhenNoTestsLeft()
			{
				var stopwatch = new Stopwatch();
				stopwatch.Start();

				ScenarioHelper.Do(() =>
					GivenATestConsoleScenario(TestName)
						.Expect("Waiting for 2 seconds...", TimeSpan.FromSeconds(0.2))
						.Run(TimeSpan.FromSeconds(0.2)),
					ScenarioHelper.Expect(
						"Process wait for exit timeout",
						2,
						null,
						null
						));

				stopwatch.Stop();
				Assert.That(stopwatch.Elapsed, Is.LessThan(TimeSpan.FromSeconds(2)), "Console should be killed if waiting for too long");
			}
		}

		private static IScenario GivenATestConsoleScenario(string testName)
		{
			var codebasePath = Path.GetDirectoryName(new Uri(typeof (EndToEndTests).Assembly.CodeBase).LocalPath);

			if (codebasePath == null)
				throw new NullReferenceException("The LocalPath of the ConsoleScenario.Tests assembly resolved to null");

			var appPath = Path.Combine(codebasePath, @"ConsoleScenario.TestApp.exe");

			return Scenarios.Create(appPath, testName);
		}
	}
}
