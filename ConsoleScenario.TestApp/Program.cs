using System;
using System.Linq;

namespace ConsoleScenario.TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var testName = args.FirstOrDefault();

			var tests = Tests.All();

			if (testName == null)
			{
				Console.WriteLine("Syntax: ConsoleScenario.TestApp.exe [test-name]");
				foreach (var test in tests.Keys)
					Console.WriteLine("  " + test);
				return;
			}

			new TestRunner(tests).Run(testName);
		}
	}
}
