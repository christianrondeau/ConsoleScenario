using System;
using System.IO;

namespace ConsoleScenario.Tests.Functional
{
	public abstract class EndToEndTestsBase
	{
		protected static IScenario GivenATestConsoleScenario(string testName)
		{
			var codebasePath = Path.GetDirectoryName(new Uri(typeof(EndToEndTestsBase).Assembly.CodeBase).LocalPath);

			if (codebasePath == null)
				throw new NullReferenceException("The LocalPath of the ConsoleScenario.Tests assembly resolved to null");

			var appPath = Path.Combine(codebasePath, @"ConsoleScenario.TestApp.exe");

			return Scenarios.Create(appPath, testName);
		}
	}
}
