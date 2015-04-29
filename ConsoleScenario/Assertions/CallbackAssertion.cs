using System;

namespace ConsoleScenario.Assertions
{
	public class CallbackAssertion : IAssertion
	{
		private readonly Func<string, bool> _callback;

		public CallbackAssertion(Func<string, bool> callback)
		{
			if (callback == null) throw new ArgumentNullException("callback");
			_callback = callback;
		}

		public AssertionResult Assert(int lineIndex, string actualLine)
		{
			if (actualLine == null)
				throw new ScenarioAssertionException("Missing line", lineIndex, null, null);

			if (!_callback(actualLine))
				throw new ScenarioAssertionException("Unexpected line", lineIndex, actualLine, _callback.Method.Name);

			return AssertionResult.MoveToNextAssertion;
		}
	}
}