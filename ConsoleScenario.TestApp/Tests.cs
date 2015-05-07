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
				{"three-lines", ThreeLines},
				{"timeout", Timeout},
				{"print-input", PrintInput},
				{"count-to-ten", CountToTen},
				{"yes-no", YesNo}
			};
		}

		public static void OneLine()
		{
			Console.WriteLine("Single line output.");
		}

		public static void TwoLines()
		{
			Console.WriteLine("Line 1");
			Console.WriteLine("Line 2");
		}

		public static void ThreeLines()
		{
			Console.WriteLine("This is the first line.");
			Console.WriteLine("This is the middle line.");
			Console.WriteLine("This is the last line.");
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

		public static void CountToTen()
		{
			for (var i = 1; i <= 10; i++)
				Console.WriteLine(i);
		}

		public static void YesNo()
		{
			Console.Write("Do you want to continue? (y/n): ");
			var value = Console.ReadLine();
			Console.WriteLine("You have selected {0}", value == "y" ? "yes" : "no");
		}
	}
}