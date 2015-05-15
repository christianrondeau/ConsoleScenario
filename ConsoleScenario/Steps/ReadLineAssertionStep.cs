using System;
using ConsoleScenario.StreamHandling;

namespace ConsoleScenario.Steps
{
	public class ReadLineAssertionStep : IScenarioStep
	{
		public IAssertion Assertion { get; private set; }
		public TimeSpan Timeout { get; protected set; }
		public int Repeat { get; private set; }

		public ReadLineAssertionStep WithTimeout(TimeSpan timeout)
		{
			Timeout = timeout;
			return this;
		}

		public ReadLineAssertionStep Times(int times)
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
				actualLine = ReadHelper.WithChecks(() => ReadLine(asyncDuplexStreamHandler), lineIndex);
				var assertionResult = Assertion.Assert(actualLine);

				if (assertionResult.Success) continue;

				var error = asyncDuplexStreamHandler.ReadError(Timeout);
				if (!String.IsNullOrEmpty(error))
					throw new ScenarioAssertionException("Unexpected console error", lineIndex, error, null);

				throw new ScenarioAssertionException(assertionResult.Message, lineIndex, actualLine, assertionResult.Expected);
			} while (--repeat > 0 && actualLine != null);
		}

		protected virtual string ReadLine(IAsyncDuplexStreamHandler asyncDuplexStreamHandler)
		{
			return asyncDuplexStreamHandler.ReadLine(Timeout);
		}
	}
}