using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleScenario.TestApp
{
	public static class Tests
	{
		public static IDictionary<string, Action> All()
		{
			return new Dictionary<string, Action>
			{
				{"one-line", OneLine},
				{"two-lines", TwoLines},
				{"timeout", Timeout},
				{"print-input", PrintInput}
			};
		}

		public static void TwoLines()
		{
			Console.WriteLine("Line 1");
			Console.WriteLine("Line 2");
		}

		public static void OneLine()
		{
			Console.WriteLine("Single line output.");
		}

		public static void Timeout()
		{
			Console.WriteLine("Waiting for 2 seconds...");
			Thread.Sleep(TimeSpan.FromSeconds(120));
			Console.WriteLine("Waiting complete.");
		}

		public static void PrintInput()
		{
			Console.WriteLine("Enter a value:");
			var value = Console.ReadLine();
			Console.WriteLine("You have entered: " + value);
		}
	}
}