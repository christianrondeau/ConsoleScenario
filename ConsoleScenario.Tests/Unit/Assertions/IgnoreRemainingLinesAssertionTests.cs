using ConsoleScenario.Assertions;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Unit.Assertions
{
	public class IgnoreRemainingLinesAssertionTests
	{
		[Test]
		public void DoesNothingIfLinesMatch()
		{
			Assert.That(
				new IgnoreRemainingLinesAssertion().Assert(0, "This should not matter"),
				Is.EqualTo(AssertionResult.KeepUsingSameAssertion));
		}
	}
}
