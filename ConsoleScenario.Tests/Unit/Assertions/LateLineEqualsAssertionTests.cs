using ConsoleScenario.Assertions;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Unit.Assertions
{
	public class LateLineEqualsAssertionTests
	{
		[Test]
		public void EqualStringsPass()
		{
			Assert.That(
				new LateLineEqualsAssertion(() => "Expected").Assert("Expected").Success,
				Is.True);
		}

		[Test]
		public void DifferentStringsFail()
		{
			Assert.That(
				new LateLineEqualsAssertion(() => "Expected").Assert("Actual"),
				Is.EqualTo(new AssertionResult
				{
					Success = false,
					Message = "Unexpected line",
					Expected = "Expected"
				}));
		}

		[Test]
		public void NullFail()
		{
			Assert.That(
				new LateLineEqualsAssertion(() => "Expected").Assert(null),
				Is.EqualTo(new AssertionResult
				{
					Success = false,
					Message = "Missing line",
					Expected = "Expected"
				}));
		}
	}
}
