using ConsoleScenario.Assertions;
using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Unit.Assertions
{
	public class AnyLineAssertionTests
	{
		[Test]
		public void ReturnsMoveToNextAssertionUntilNoLinesLeft()
		{
			var assertion = new AnyLineAssertion(2);

			Assert.That(
				assertion.Assert(0, "Anything"),
				Is.EqualTo(AssertionResult.KeepUsingSameAssertion));

			Assert.That(
				assertion.Assert(0, "Anything"),
				Is.EqualTo(AssertionResult.MoveToNextAssertion));
		}

		[Test]
		public void ThrowsIfNoLinesLeft()
		{
			ScenarioHelper
				.Do(() => new AnyLineAssertion(1).Assert(15, null),
					ScenarioHelper.Expect("Missing line", 16, null, null));
		}
	}
}