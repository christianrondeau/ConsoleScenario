using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests
{
	public class ScenarioAssertionExceptionTests
	{
		[Test]
		public void CanFormatErrorInformation_WithActualAndExpected()
		{
			ScenarioHelper.Do<ScenarioAssertionException>(
				() => { throw new ScenarioAssertionException("description", 5, "actual", "expected"); },
				exc =>
				{
					Assert.That(exc.Description, Is.EqualTo("description"));
					Assert.That(exc.LineIndex, Is.EqualTo(5));
					Assert.That(exc.Actual, Is.EqualTo("actual"));
					Assert.That(exc.Expected, Is.EqualTo("expected"));
					Assert.That(exc.Message, Is.EqualTo("Assert failed at line 6: description\r\n--- Received:\r\nactual\r\n--- Expected:\r\nexpected"));
				});
		}

		[Test]
		public void CanFormatErrorInformation_WithActualOnly()
		{
			ScenarioHelper.Do<ScenarioAssertionException>(
				() => { throw new ScenarioAssertionException("description", 5, "actual", null); },
				exc =>
				{
					Assert.That(exc.Description, Is.EqualTo("description"));
					Assert.That(exc.LineIndex, Is.EqualTo(5));
					Assert.That(exc.Actual, Is.EqualTo("actual"));
					Assert.That(exc.Expected, Is.Null);
					Assert.That(exc.Message, Is.EqualTo("Assert failed at line 6: description\r\n--- Received:\r\nactual"));
				});
		}

		[Test]
		public void CanFormatErrorInformation_WithExpectedOnly()
		{
			ScenarioHelper.Do<ScenarioAssertionException>(
				() => { throw new ScenarioAssertionException("description", 5, null, "expected"); },
				exc =>
				{
					Assert.That(exc.Description, Is.EqualTo("description"));
					Assert.That(exc.LineIndex, Is.EqualTo(5));
					Assert.That(exc.Actual, Is.Null);
					Assert.That(exc.Expected, Is.EqualTo("expected"));
					Assert.That(exc.Message, Is.EqualTo("Assert failed at line 6: description\r\n--- Expected:\r\nexpected"));
				});
		}

		[Test]
		public void CanFormatErrorInformation_WithNeitherActualNorExpected()
		{
			ScenarioHelper.Do<ScenarioAssertionException>(
				() => { throw new ScenarioAssertionException("description", 5, null, null); },
				exc =>
				{
					Assert.That(exc.Description, Is.EqualTo("description"));
					Assert.That(exc.LineIndex, Is.EqualTo(5));
					Assert.That(exc.Actual, Is.Null);
					Assert.That(exc.Expected, Is.Null);
					Assert.That(exc.Message, Is.EqualTo("Assert failed at line 6: description"));
				});
		}
	}
}