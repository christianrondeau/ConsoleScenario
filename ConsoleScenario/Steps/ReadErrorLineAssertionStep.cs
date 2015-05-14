using System;

namespace ConsoleScenario.Steps
{
	public class ReadErrorLineAssertionStep : ReadAssertionStepBase
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