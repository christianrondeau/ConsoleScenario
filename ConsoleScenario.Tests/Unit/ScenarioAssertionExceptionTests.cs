using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests
{
	public class ScenarioAssertionExceptionTests
	{
		[Test]
		public void CanFormatErrorInformation()
		{
			ScenarioHelper.Do<ScenarioAssertionException>(
				() => { throw new ScenarioAssertionException("description", "actual", "expected"); },
				exc =>
				{
					Assert.That(exc.Description, Is.EqualTo("description"));
					Assert.That(exc.Actual, Is.EqualTo("actual"));
					Assert.That(exc.Expected, Is.EqualTo("expected"));
					Assert.That(exc.Message, Is.EqualTo("description\r\n--- Received:\r\nactual\r\n--- Expected:\r\nexpected"));
				});
		}
	}
}