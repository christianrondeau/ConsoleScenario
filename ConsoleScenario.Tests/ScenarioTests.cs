using System;
using System.IO;
using NUnit.Framework;

namespace ConsoleScenario.Tests
{
	[TestFixture]
    public class ScenarioTests
    {
		[Test]
		public void OneLine()
		{
			var appPath = Path.Combine(Path.GetDirectoryName(new Uri(typeof(ScenarioTests).Assembly.CodeBase).LocalPath), @"ConsoleScenario.TestApp.exe");

			var scenario = new Scenario(appPath, "one-line")
				.Expect("Single line output.");

			scenario.Run();
		}
    }
}
