using System;

namespace ConsoleScenario.Assertions
{
	public class LineContainsAssertion : IAssertion
	{
		private readonly string _expectedString;

		public LineContainsAssertion(string expectedString)
		{
			if (expectedString == null) throw new ArgumentNullException("expectedString");
			_expectedString = expectedString;
		}

		public AssertionResult Assert(string actualLine)
		{
			if (actualLine == null)
				return AssertionResult.Fail("Missing line", _expectedString);

			return !actualLine.Contains(_expectedString)
				? AssertionResult.Fail("Unexpected line content", _expectedString)
				: AssertionResult.Pass();
		}
	}
}