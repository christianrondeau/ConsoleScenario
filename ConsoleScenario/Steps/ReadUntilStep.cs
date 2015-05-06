using System;

namespace ConsoleScenario.Steps
{
	public interface IReadUntilStep : IScenarioStep
	{
		IReadUntilStep WithTimeout(TimeSpan timeout);
	}

	public class ReadUntilStep : ReadStepBase, IReadUntilStep
	{
		private readonly Func<string, bool> _condition;

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

		public void Run(IAsyncDuplexStreamHandler asyncDuplexStreamHandler, ref int lineIndex)
		{
			string actualLine;

			do
			{
				lineIndex++;
				actualLine = ReadLineOrTimeout(lineIndex, asyncDuplexStreamHandler);

				if (_condition(actualLine))
					return;

			} while (!_condition(actualLine) && actualLine != null);

			throw new ScenarioAssertionException("Until was never true", lineIndex, actualLine, null);
		}
	}
}