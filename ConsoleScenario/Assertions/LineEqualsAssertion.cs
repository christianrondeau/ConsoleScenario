using System;

namespace ConsoleScenario.Assertions
{
	public class LineEqualsAssertion : IAssertion
	{
		private readonly string _expectedLine;

		public LineEqualsAssertion(string expectedLine)
		{
			if (expectedLine == null) throw new ArgumentNullException("expectedLine");	
			_expectedLine = expectedLine;
		}

		public AssertionResult Assert(string actualLine)
		{
			if (actualLine == null)
				return AssertionResult.Fail("Missing line", _expectedLine);

			return actualLine != _expectedLine
				? AssertionResult.Fail("Unexpected line", _expectedLine)
				: AssertionResult.Pass();
		}
	}
}