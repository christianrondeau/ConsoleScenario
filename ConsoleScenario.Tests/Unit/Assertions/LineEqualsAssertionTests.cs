using ConsoleScenario.Assertions;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Unit.Assertions
{
	public class LineEqualsAssertionTests
	{
		[Test]
		public void DoesNothingIfLinesMatch()
		{
			Assert.That(
				new LineEqualsAssertion("Expected").Assert("Expected").Success,
				Is.True);
		}

		[Test]
		public void DifferentStringsFail()
		{
			Assert.That(
				new LineEqualsAssertion("Expected").Assert("Actual"),
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
				new LineEqualsAssertion("Expected").Assert(null),
				Is.EqualTo(new AssertionResult
				{
					Success = false,
					Message = "Missing line",
					Expected = "Expected"
				}));
		}
	}
}
