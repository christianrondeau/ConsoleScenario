using System;

namespace ConsoleScenario.Assertions
{
	public abstract class AssertionBase
	{
		public TimeSpan Timeout { get; protected set; }

		protected AssertionBase()
			: this(ConsoleScenarioDefaults.Timeout)
		{
		}

		protected AssertionBase(TimeSpan timeout)
		{
			Timeout = timeout;
		}
	}
}