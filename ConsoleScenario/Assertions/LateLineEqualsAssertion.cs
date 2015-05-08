using System;

namespace ConsoleScenario.Assertions
{
	public class LateLineEqualsAssertion : IAssertion
	{
		private readonly Func<string> _expectedLineFn;

		public LateLineEqualsAssertion(Func<string> expectedLineFn)
		{
			if (expectedLineFn == null) throw new ArgumentNullException("expectedLineFn");
			_expectedLineFn = expectedLineFn;
		}

		public AssertionResult Assert(string actualLine)
		{
			var expectedLine = _expectedLineFn();

			if (actualLine == null)
				return AssertionResult.Fail("Missing line", expectedLine);

			return actualLine != expectedLine
				? AssertionResult.Fail("Unexpected line", expectedLine)
				: AssertionResult.Pass();
		}
	}
}