using System;
using System.Linq;

namespace ConsoleScenario.TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var testName = args.FirstOrDefault();

			switch (testName)
			{
				case "one-line":
					OneLine();
					return;
				default:
					throw new ApplicationException(string.Format("Unknown conosle argument: {0}", testName));
			}
		}

		private static void OneLine()
		{
			Console.WriteLine("Single line output.");
		}
	}
}
