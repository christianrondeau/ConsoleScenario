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

		public AssertionResult Assert(string actualLine)
		{
			if (actualLine == null)
				return AssertionResult.Fail("Missing line");

			return !_callback(actualLine)
				? AssertionResult.Fail("Unexpected line", _callback.Method.Name)
				: AssertionResult.Pass();
		}
	}
}