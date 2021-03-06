using System;
using System.Runtime.Serialization;

namespace ConsoleScenario
{
	[Serializable]
	public class ScenarioAssertionException : ApplicationException
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
			: this(description, lineIndex, actual, expected, null)
		{
		}

		public ScenarioAssertionException(string description, int lineIndex, string actual, string expected, Exception innerException)
			: base(BuildMessage(description, lineIndex, actual, expected), innerException)
		{
			if (description == null) throw new ArgumentNullException("description");

			_description = description;
			_lineIndex = lineIndex;
			_actual = actual;
			_expected = expected;
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Description", _description);
			info.AddValue("LineIndex", _lineIndex);
			info.AddValue("Actual", _actual);
			info.AddValue("Expected", _expected);
		}

		private static string BuildMessage(string description, int lineIndex, string actual, string expected)
		{
			var basicMessage = String.Format("Assert failed at line {0}: {1}", lineIndex + 1, description);

			if (actual != null && expected != null)
				return String.Format("{1}{0}--- Received:{0}{2}{0}--- Expected:{0}{3}",
					Environment.NewLine,
					basicMessage,
					actual,
					expected);

			if (expected != null)
				return String.Format("{1}{0}--- Expected:{0}{2}",
					Environment.NewLine,
					basicMessage,
					expected);

			if (actual != null)
				return String.Format("{1}{0}--- Received:{0}{2}",
					Environment.NewLine,
					basicMessage,
					actual);

			return basicMessage;

		}
	}
}