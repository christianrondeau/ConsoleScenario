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

		private static IScenario GivenATestConsoleScenario()
		{
			var appPath = Path.Combine(Path.GetDirectoryName(new Uri(typeof(EndToEndTests).Assembly.CodeBase).LocalPath),
				@"ConsoleScenario.TestApp.exe");

			return Scenarios.Create(appPath, "one-line");
		}
	}
}
