using System;
using System.Collections.Generic;

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
	}
}