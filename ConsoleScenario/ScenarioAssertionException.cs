using System;

namespace ConsoleScenario
{
	public class ScenarioAssertionException : Exception
	{
		private readonly string _description;
		private readonly int _lineIndex;
		private readonly string _actual;
		private readonly string _expected;

		public string Description
		{
			get { return _description; }
		}

		public int LineIndex
		{
			get { return _lineIndex; }
		}

		public string Actual
		{
			get { return _actual; }
		}

		public string Expected
		{
			get { return _expected; }
		}

		public ScenarioAssertionException(string description, int lineIndex, string actual, string expected)
			: base(BuildMessage(description, lineIndex, actual, expected))
		{
			if (description == null) throw new ArgumentNullException("description");
			if (actual == null) throw new ArgumentNullException("actual");
			if (expected == null) throw new ArgumentNullException("expected");
			if (lineIndex < 0) throw new ArgumentException("Line index must be at least 0", "lineIndex");

			_description = description;
			_lineIndex = lineIndex;
			_actual = actual;
			_expected = expected;
		}

		private static string BuildMessage(string description, int lineIndex, string actual, string expected)
		{
			return String.Format("Assert failed at line {1}: {2}{0}--- Received:{0}{3}{0}--- Expected:{0}{4}",
				Environment.NewLine,
				lineIndex + 1,
				description,
				actual,
				expected);
		}
	}
}