using System;
using ConsoleScenario.StreamHandling;

namespace ConsoleScenario.Steps
{
	public class ReadErrorLineAssertionStep : ReadLineAssertionStep
	{
		public ReadErrorLineAssertionStep(IAssertion assertion)
			: base(assertion)
		{	
		}

		protected override string ReadLine(IAsyncDuplexStreamHandler asyncDuplexStreamHandler)
		{
			var line = asyncDuplexStreamHandler.ReadError(Timeout);

			if (String.IsNullOrWhiteSpace(line))
				line = asyncDuplexStreamHandler.ReadError(Timeout);

			return line;
		}
	}
}