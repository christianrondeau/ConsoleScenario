using ConsoleScenario.Assertions;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Unit.Assertions
{
	public class LineContainsAssertionTests
	{
		[Test]
		public void EqualStringsPass()
		{
			Assert.That(
				new LineContainsAssertion("Expected").Assert("Expected").Success,
				Is.True);
		}

		[Test]
		public void StringSubsetPass()
		{
			Assert.That(
				new LineContainsAssertion("line").Assert("Full line text").Success,
				Is.True);
		}

		[Test]
		public void DifferentStringsFail()
		{
			Assert.That(
				new LineContainsAssertion("Expected").Assert("Actual"),
				Is.EqualTo(new AssertionResult
				{
					Success = false,
					Message = "Unexpected line content",
					Expected = "Expected"
				}));
		}

		[Test]
		public void NullFail()
		{
			Assert.That(
				new LineContainsAssertion("Expected").Assert(null),
				Is.EqualTo(new AssertionResult
				{
					Success = false,
					Message = "Missing line",
					Expected = "Expected"
				}));
		}
	}
}