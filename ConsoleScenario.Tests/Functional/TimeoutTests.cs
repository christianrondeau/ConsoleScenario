using System;
using System.Diagnostics;
using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Functional
{
	public class TimeoutTests : EndToEndTestsBase
	{
		private const string TestName = "timeout";

		[Test]
		public void FailureBecauseConsoleDidNotRespond()
		{
			ScenarioHelper.Do(() =>
				GivenATestConsoleScenario(TestName)
					.Expect("Waiting for 2 seconds...")
					.Expect("This line will never come.", TimeSpan.FromSeconds(0.5))
					.Run(TimeSpan.FromSeconds(0.5)),
				ScenarioHelper.Expect(
					"Timeout",
					2,
					null,
					(string)null
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
					(string)null
					));

			stopwatch.Stop();
			Assert.That(stopwatch.Elapsed, Is.LessThan(TimeSpan.FromSeconds(2)), "Console should be killed if waiting for too long");
		}
	}
}