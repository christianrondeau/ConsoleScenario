using System;

namespace ConsoleScenario.Steps
{
	public interface IReadAssertionStep : IScenarioStep
	{
		IReadAssertionStep WithTimeout(TimeSpan timeout);
		IReadAssertionStep Times(int times);
	}

	public class ReadLineAssertionStep : ReadStepBase, IReadAssertionStep
	{
		public IAssertion Assertion { get; private set; }
		public int Repeat { get; private set; }

		public IReadAssertionStep WithTimeout(TimeSpan timeout)
		{
			Timeout = timeout;
			return this;
		}

		public IReadAssertionStep Times(int times)
		{
			Repeat = times;
			return this;
		}

		public ReadLineAssertionStep(IAssertion assertion)
		{
			if (assertion == null) throw new ArgumentNullException("assertion");

			Assertion = assertion;
		}

		public void Run(IAsyncDuplexStreamHandler asyncDuplexStreamHandler, ref int lineIndex)
		{
			var repeat = Repeat;
			string actualLine;

			do
			{
				lineIndex++;
				actualLine = ReadLineOrTimeout(lineIndex, asyncDuplexStreamHandler);
				var assertionResult = Assertion.Assert(actualLine);

				if (!assertionResult.Success)
					throw new ScenarioAssertionException(assertionResult.Message, lineIndex, actualLine, assertionResult.Expected);
			} while (--repeat > 0 && actualLine != null);
		}
	}
}
