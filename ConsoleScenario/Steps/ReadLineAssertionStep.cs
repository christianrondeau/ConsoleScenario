namespace ConsoleScenario.Steps
{
	public class ReadLineAssertionStep : ReadAssertionStepBase
	{
		public ReadLineAssertionStep(IAssertion assertion) : base(assertion)
		{	
		}

		protected override string ReadLine(IAsyncDuplexStreamHandler asyncDuplexStreamHandler)
		{
			return asyncDuplexStreamHandler.ReadLine(Timeout);
		}
	}
}
