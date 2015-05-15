using System;
using ConsoleScenario.StreamHandling;

namespace ConsoleScenario.Steps
{
	public class ReadUntilStep : IScenarioStep
	{
		public TimeSpan Timeout { get; private set; }
		private readonly Func<string, bool> _condition;

		public ReadUntilStep WithTimeout(TimeSpan timeout)
		{
			Timeout = timeout;
			return this;
		}

		public ReadUntilStep(Func<string, bool> condition)
		{
			if (condition == null) throw new ArgumentNullException("condition");
			_condition = condition;
		}

		public void Run(IAsyncDuplexStreamHandler asyncDuplexStreamHandler, ref int lineIndex)
		{
			string actualLine;

			do
			{
				lineIndex++;
				actualLine = ReadHelper.WithChecks(() => asyncDuplexStreamHandler.ReadLine(Timeout), lineIndex);

				if (_condition(actualLine))
					return;

			} while (!_condition(actualLine) && actualLine != null);

			throw new ScenarioAssertionException("Until was never true", lineIndex, actualLine, null);
		}
	}
}