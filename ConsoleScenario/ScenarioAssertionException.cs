using System;

namespace ConsoleScenario
{
	public class ScenarioAssertionException : Exception
	{
		private readonly string _description;
		private readonly string _actual;
		private readonly string _expected;

		public string Description
		{
			get { return _description; }
		}

		public string Actual
		{
			get { return _actual; }
		}

		public string Expected
		{
			get { return _expected; }
		}

		public ScenarioAssertionException(string description, string actual, string expected)
			: base(BuildMessage(description, actual, expected))
		{
			_description = description;
			_actual = actual;
			_expected = expected;
		}

		private static string BuildMessage(string description, string actual, string expected)
		{
			return String.Format("{1}{0}--- Received:{0}{2}{0}--- Expected:{0}{3}",
				Environment.NewLine,
				description,
				actual,
				expected);
		}
	}
}