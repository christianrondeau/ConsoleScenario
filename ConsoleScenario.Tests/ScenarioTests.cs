using System;
using System.IO;
using NUnit.Framework;

namespace ConsoleScenario.Tests
{
	public class ScenarioTests
	{
		public class OneLineTests
		{
			[Test]
			public void Success()
			{
				GivenATestConsoleScenario()
					.Expect("Single line output.")
					.Run();
			}

			[Test]
			public void Failure()
			{
				ScenarioHelper.Do(() =>
					GivenATestConsoleScenario()
						.Expect("This is not what I expected...")
						.Run(),
					ScenarioHelper.Expect(
						"Invalid console output",
						"Single line output.",
						"This is not what I expected..."
						));
			}
		}

		private static Scenario GivenATestConsoleScenario()
		{
			var appPath = Path.Combine(Path.GetDirectoryName(new Uri(typeof(ScenarioTests).Assembly.CodeBase).LocalPath),
				@"ConsoleScenario.TestApp.exe");

			var scenario = new Scenario(appPath, "one-line");
			return scenario;
		}
	}
}
