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
		private readonly string _value;

		public TimeSpan Timeout { get; private set; }

		public IReadCharStep WithTimeout(TimeSpan timeout)
		{
			Timeout = timeout;
			return this;
		}

		public ReadCharsStep(string value)
		{
			if (value == null) throw new ArgumentNullException("value");

			_value = value;
		}

		public void Run(IAsyncDuplexStreamHandler asyncDuplexStreamHandler, ref int lineIndex)
		{

			var charIndex = 0;
			foreach (var expectedChar in _value)
			{
				try
				{
					var actualChar = asyncDuplexStreamHandler.ReadChar(Timeout);
					ValidateChar(lineIndex, actualChar, expectedChar, charIndex++);
				}
				catch (TimeoutException exc)
				{
					throw new ScenarioAssertionException("Timeout", lineIndex, null, _value, exc);
				}
			}
		}

		private void ValidateChar(int lineIndex, char actual, char expected, int position)
		{
			switch (actual)
			{
				case char.MinValue:
					throw new ScenarioAssertionException("Unexpected end of stream", lineIndex, null, _value);
				case '\r':
				case '\n':
					throw new ScenarioAssertionException("Unexpected end of line", lineIndex, null, _value);
			}

			if (actual != expected)
				throw new ScenarioAssertionException(
					string.Format("Unexpected prompt at character {0}", position),
					lineIndex,
					actual.ToString(),
					_value);
		}
	}
}