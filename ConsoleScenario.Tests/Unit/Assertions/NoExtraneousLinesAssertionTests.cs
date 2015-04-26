using ConsoleScenario.Assertions;
using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Unit.Assertions
{
	public class NoExtraneousLinesAssertionTests
	{
		[Test]
		public void DoesNothingIfLinesMatch()
		{
			Assert.That(
				new NoExtraneousLinesAssertion().Assert(0, null),
				Is.EqualTo(AssertionResult.KeepUsingSameAssertion));
		}

		[Test]
		public void ThrowsWhenStringsDoNotMatch()
		{
			ScenarioHelper
				.Do(() => new NoExtraneousLinesAssertion().Assert(5, "Unexpected"),
					ScenarioHelper.Expect("Extraneous line", 6, "Unexpected", null));
		}
	}
}