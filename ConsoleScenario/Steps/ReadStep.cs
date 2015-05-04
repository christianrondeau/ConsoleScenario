using System;

namespace ConsoleScenario.Steps
{
	public interface IReadStep : IScenarioStep
	{
		IReadStep WithTimeout(TimeSpan timeout);
		IReadStep Times(int times);
	}

	public class ReadStep : IReadStep
	{
		public TimeSpan Timeout { get; private set; }
		public IAssertion Assertion { get; private set; }
		public int Repeat { get; private set; }

		public IReadStep WithTimeout(TimeSpan timeout)
		{
			Timeout = timeout;
			return this;
		}

		public IReadStep Times(int times)
		{
			Repeat = times;
			return this;
		}

		public ReadStep(IAssertion assertion)
		{
			if (assertion == null) throw new ArgumentNullException("assertion");

			Assertion = assertion;
		}

		public void Run(IAsyncDuplexStreamHandler asyncTwoWayStreamsHandler, ref int lineIndex)
		{
			var repeat = Repeat;
			string actualLine;

			do
			{
				lineIndex++;
				actualLine = ReadLineOrTimeout(lineIndex, asyncTwoWayStreamsHandler, Timeout);
				var assertionResult = Assertion.Assert(actualLine);

				if (!assertionResult.Success)
					throw new ScenarioAssertionException(assertionResult.Message, lineIndex, actualLine, assertionResult.Expected);
			} while (--repeat > 0 && actualLine != null);
		}

		private static string ReadLineOrTimeout(int lineIndex, IAsyncDuplexStreamHandler asyncTwoWayStreamsHandler, TimeSpan timeout)
		{
			try
			{
				return asyncTwoWayStreamsHandler.ReadLine(timeout);
			}
			catch (TimeoutException exc)
			{
				throw new ScenarioAssertionException("Timeout", lineIndex, null, null, exc);
			}
		}
	}

	public interface IReadUntilStep : IScenarioStep
	{
		IReadUntilStep WithTimeout(TimeSpan timeout);
	}

	public class ReadUntilStep : IReadUntilStep
	{
		private readonly Func<string, bool> _condition;
		public TimeSpan Timeout { get; private set; }

		public IReadUntilStep WithTimeout(TimeSpan timeout)
		{
			Timeout = timeout;
			return this;
		}

		public ReadUntilStep(Func<string, bool> condition)
		{
			if (condition == null) throw new ArgumentNullException("condition");
			_condition = condition;
		}

		public void Run(IAsyncDuplexStreamHandler asyncTwoWayStreamsHandler, ref int lineIndex)
		{
			string actualLine;

			do
			{
				lineIndex++;
				actualLine = ReadLineOrTimeout(lineIndex, asyncTwoWayStreamsHandler, Timeout);

				if (_condition(actualLine))
					return;

			} while (!_condition(actualLine) && actualLine != null);

			throw new ScenarioAssertionException("Until was never true", lineIndex, actualLine, null);
		}

		private static string ReadLineOrTimeout(int lineIndex, IAsyncDuplexStreamHandler asyncTwoWayStreamsHandler, TimeSpan timeout)
		{
			try
			{
				return asyncTwoWayStreamsHandler.ReadLine(timeout);
			}
			catch (TimeoutException exc)
			{
				throw new ScenarioAssertionException("Timeout", lineIndex, null, null, exc);
			}
		}
	}
}
