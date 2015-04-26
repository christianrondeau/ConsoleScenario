using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Unit
{
	public class LineEqualsAssertionTests
	{
		[Test]
		public void DoesNothingIfLinesMatch()
		{
			new LineEqualsAssertion("Expected").Assert(0, "Expected");
			Assert.Pass("No exception thrown when strings match");
		}


		[Test]
		public void ThrowsWhenStringsDoNotMatch()
		{
			ScenarioHelper
				.Do(() => new LineEqualsAssertion("Expected").Assert(1, "Actual"),
					ScenarioHelper.Expect("Unexpected line", 2, "Actual", "Expected"));
		}


		[Test]
		public void ThrowsWhenActualIsNull()
		{
			ScenarioHelper
				.Do(() => new LineEqualsAssertion("Expected").Assert(122, null),
					ScenarioHelper.Expect("Missing line", 123, null, "Expected"));
		}
	}
}
