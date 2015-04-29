using ConsoleScenario.Assertions;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Unit.Assertions
{
	public class CallbackAssertionTests
	{
		[Test]
		public void ReturnTruePass()
		{
			Assert.That(
				new CallbackAssertion(line => line == "Actual").Assert("Actual").Success,
				Is.True);
		}

		[Test]
		public void ReturnFalseFail()
		{
			Assert.That(
				new CallbackAssertion(line => false).Assert("Actual"),
				Is.EqualTo(new AssertionResult
				{
					Success = false,
					Message = "Unexpected line",
					Expected = "<ReturnFalseFail>b__3"
				}));
		}

		[Test]
		public void NullFail()
		{
			Assert.That(
				new CallbackAssertion(line => true).Assert(null),
				Is.EqualTo(new AssertionResult
				{
					Success = false,
					Message = "Missing line",
					Expected = "<NullFail>b__6"
				}));
		}
	}
}