using System;

namespace ConsoleScenario
{
	public class ConsoleScenarioDefaults
	{
		public static TimeSpan Timeout { get; set; }

		static ConsoleScenarioDefaults()
		{
			Timeout = TimeSpan.FromSeconds(2);
		}
	}
}