using ConsoleScenario.Assertions;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Unit.Assertions
{
	public class IgnoreLineAssertionTests
	{
		[Test]
		public void AnyStringPass()
		{
			Assert.That(
				new IgnoreLineAssertion().Assert("Anything").Success,
				Is.True);
		}

		[Test]
		public void NullPass()
		{
			Assert.That(
				new IgnoreLineAssertion().Assert(null).Success,
				Is.True);
		}
	}
}