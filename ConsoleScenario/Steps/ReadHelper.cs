using System;

namespace ConsoleScenario.Steps
{
	public static class ReadHelper
	{
		public static T WithChecks<T>(Func<T> fn, int lineIndex)
		{
			try
			{
				return fn();
			}
			catch (ApplicationException exc)
			{
				throw new ScenarioAssertionException("Unexpected console error", lineIndex, exc.Message, null, exc);
			}
			catch (TimeoutException exc)
			{
				throw new ScenarioAssertionException("Timeout", lineIndex, null, null, exc);
			}
		}
	}
}