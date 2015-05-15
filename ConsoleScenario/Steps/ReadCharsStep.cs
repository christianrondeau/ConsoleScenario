using System;
using System.Text;

namespace ConsoleScenario.Steps
{
	public interface IReadCharStep : IScenarioStep
	{
		IReadCharStep WithTimeout(TimeSpan timeout);
	}

	public class ReadCharsStep : IReadCharStep
	{
		private readonly string _expected;

		public TimeSpan Timeout { get; private set; }

		public IReadCharStep WithTimeout(TimeSpan timeout)
		{
			Timeout = timeout;
			return this;
		}

		public ReadCharsStep(string expected)
		{
			if (expected == null) throw new ArgumentNullException("expected");

			_expected = expected;
		}

		public void Run(IAsyncDuplexStreamHandler asyncDuplexStreamHandler, ref int lineIndex)
		{
			lineIndex++;
			var sb = new StringBuilder();

			var charIndex = 0;
			foreach (var expectedChar in _expected)
			{
				var actualChar = ReadHelper.WithChecks(() => asyncDuplexStreamHandler.Read(Timeout), lineIndex);
				ValidateChar(lineIndex, actualChar, expectedChar, charIndex++, sb);
				sb.Append(actualChar);
			}
		}

		// ReSharper disable once UnusedParameter.Local
		private void ValidateChar(int lineIndex, char actual, char expected, int position, StringBuilder sb)
		{
			switch (actual)
			{
				case char.MinValue:
					throw new ScenarioAssertionException("Unexpected end of stream", lineIndex, GetStringBuildValueOrNull(sb), _expected);
				case '\r':
				case '\n':
					throw new ScenarioAssertionException("Unexpected end of line", lineIndex, GetStringBuildValueOrNull(sb), _expected);
			}

			if (actual != expected)
				throw new ScenarioAssertionException(
					string.Format("Unexpected prompt at character {0}", position),
					lineIndex,
					GetStringBuildValueOrNull(sb),
					_expected);
		}

		private static string GetStringBuildValueOrNull(StringBuilder sb)
		{
			return sb.Length > 0 ? sb.ToString() : null;
		}
	}
}