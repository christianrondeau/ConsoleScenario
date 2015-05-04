using System;

namespace ConsoleScenario.Steps
{
	public abstract class ReadStepBase
	{
		public TimeSpan Timeout { get; protected set; }

		protected string ReadLineOrTimeout(int lineIndex, IAsyncDuplexStreamHandler asyncTwoWayStreamsHandler)
		{
			try
			{
				return asyncTwoWayStreamsHandler.ReadLine(Timeout);
			}
			catch (TimeoutException exc)
			{
				throw new ScenarioAssertionException("Timeout", lineIndex, null, null, exc);
			}
		}
	}
}