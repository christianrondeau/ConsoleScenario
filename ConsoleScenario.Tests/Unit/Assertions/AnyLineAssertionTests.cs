using ConsoleScenario.Assertions;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Unit.Assertions
{
	public class AnyLineAssertionTests
	{
		[Test]
		public void AnyStringPass()
		{
			Assert.That(
				new AnyLineAssertion().Assert("Anything").Success,
				Is.True);
		}

		[Test]
		public void NullFail()
		{
			Assert.That(
				new AnyLineAssertion().Assert(null),
				Is.EqualTo(new AssertionResult
				{
					Success = false,
					Message = "Missing line"
				}));
		}
	}
}