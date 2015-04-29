using ConsoleScenario.Assertions;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Unit.Assertions
{
	public class NoExtraneousLinesAssertionTests
	{
		[Test]
		public void NullPass()
		{
			Assert.That(
				new NoExtraneousLinesAssertion().Assert(null).Success,
				Is.True);
		}

		[Test]
		public void AnyStringFail()
		{
			Assert.That(
				new NoExtraneousLinesAssertion().Assert("Anything"),
				Is.EqualTo(new AssertionResult
				{
					Success = false,
					Message = "Extraneous line"
				}));
		}
	}
}