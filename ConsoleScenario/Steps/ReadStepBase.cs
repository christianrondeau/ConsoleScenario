using System;

namespace ConsoleScenario.Steps
{
	public abstract class ReadStepBase
	{
		public TimeSpan Timeout { get; protected set; }

		protected string ReadLineOrTimeout(int lineIndex, IAsyncDuplexStreamHandler asyncDuplexStreamHandler)
		{
			try
			{
				return asyncDuplexStreamHandler.ReadLine(Timeout);
			}
			catch (TimeoutException exc)
			{
				throw new ScenarioAssertionException("Timeout", lineIndex, null, null, exc);
			}
		}
	}
}