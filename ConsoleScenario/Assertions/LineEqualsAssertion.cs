using System;

namespace ConsoleScenario.Assertions
{
	public class LineEqualsAssertion : AssertionBase, IAssertion
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

		public LineEqualsAssertion(string expectedLine, TimeSpan timeout)
			: base(timeout)
		{
			_expectedLine = expectedLine;
			if (expectedLine == null) throw new ArgumentNullException("expectedLine");
		}

		public AssertionResult Assert(int lineIndex, string actualLine)
		{
			if (actualLine == null)
				throw new ScenarioAssertionException("Missing line", lineIndex, null, _expectedLine);

			if (actualLine != _expectedLine)
				throw new ScenarioAssertionException("Unexpected line", lineIndex, actualLine, _expectedLine);

			return AssertionResult.MoveToNextAssertion;
		}
	}
}