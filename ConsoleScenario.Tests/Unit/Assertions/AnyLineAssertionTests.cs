using ConsoleScenario.Assertions;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Unit.Assertions
{
	public class AnyLineAssertionTests
	{
		[Test]
		public void DoesNothing()
		{
			Assert.That(
				new AnyLineAssertion().Assert(0, "Anything"),
				Is.EqualTo(AssertionResult.MoveToNextAssertion));
		}
	}
}