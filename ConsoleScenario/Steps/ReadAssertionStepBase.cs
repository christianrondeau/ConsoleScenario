using System;

namespace ConsoleScenario.Steps
{
	public abstract class ReadAssertionStepBase : IReadAssertionStep
	{
		public IAssertion Assertion { get; private set; }
		public TimeSpan Timeout { get; protected set; }
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

		protected ReadAssertionStepBase(IAssertion assertion)
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

		protected abstract string ReadLine(IAsyncDuplexStreamHandler asyncDuplexStreamHandler);
	}
}