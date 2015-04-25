using System;

namespace ConsoleScenario
{
	public interface IAssertion
	{
		void Assert(int lineIndex, string actualLine);
	}

	public class LineEqualsAssertion : IAssertion
	{
		public static IAssertion Create(string expectedLine)
		{
			return new LineEqualsAssertion(expectedLine);
		}

		private readonly string _expectedLine;

		public LineEqualsAssertion(string expectedLine)
		{
			_expectedLine = expectedLine;
			if (expectedLine == null) throw new ArgumentNullException("expectedLine");
		}

		public void Assert(int lineIndex, string actualLine)
		{
			if (actualLine != _expectedLine)
				throw new ScenarioAssertionException("Unexpected line", lineIndex, actualLine, _expectedLine);
		}
	}
}