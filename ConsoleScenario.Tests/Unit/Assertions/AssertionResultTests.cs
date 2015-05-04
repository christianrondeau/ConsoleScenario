using NUnit.Framework;

namespace ConsoleScenario.Tests.Unit.Assertions
{
	public class AssertionResultTests
	{
		[Test]
		public void EqualTests()
		{
			Assert.AreEqual(
				new AssertionResult { Success = true, Message = "message", Expected = "expected" },
				new AssertionResult { Success = true, Message = "message", Expected = "expected" }
				);

			Assert.AreNotEqual(
				new AssertionResult { Success = true, Message = "message", Expected = "expected" },
				new AssertionResult { Success = false, Message = "message", Expected = "expected" }
				);

			Assert.AreNotEqual(
				new AssertionResult { Success = true, Message = "message", Expected = "expected" },
				new AssertionResult { Success = true, Message = "changed", Expected = "expected" }
				);

			Assert.AreNotEqual(
				new AssertionResult { Success = true, Message = "message", Expected = "expected" },
				new AssertionResult { Success = true, Message = "message", Expected = "changed" }
				);
		}

		[Test]
		public void ToStringTests()
		{
			Assert.That(new AssertionResult { Success = true }.ToString(), Is.EqualTo("Success"));
			Assert.That(new AssertionResult { Success = false, Message = "message" }.ToString(), Is.EqualTo("Failed: message"));
			Assert.That(new AssertionResult { Success = false, Message = "message", Expected = "expected" }.ToString(), Is.EqualTo("Failed: message (expected: 'expected')"));
		}
	}
}