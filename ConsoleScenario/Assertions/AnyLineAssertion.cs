using System;

namespace ConsoleScenario.Assertions
{
	public class AnyLineAssertion : AssertionBase, IAssertion
	{
		private int _lineCount;

		public AnyLineAssertion(int lineCount)
		{
			_lineCount = lineCount;
		}

		public AnyLineAssertion(int lineCount, TimeSpan timeout)
			: base(timeout)
		{
			_lineCount = lineCount;
		}

		public AssertionResult Assert(int lineIndex, string actualLine)
		{
			if (actualLine == null)
				throw new ScenarioAssertionException("Missing line", lineIndex, null, null);

			return --_lineCount <= 0
				? AssertionResult.MoveToNextAssertion
				: AssertionResult.KeepUsingSameAssertion;
		}
	}
}