using System;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Utils
{
	public static class ScenarioHelper
	{
		public static void Do<T>(Action action, Action<T> assert) where T : Exception
		{
			try
			{
				action();
				Assert.Fail("An exception of type {0} was expected, but no exception was thrown");
			}
			catch (Exception exc)
			{
				var expectedExceptionType = typeof(T);
				var actualExceptionType = exc.GetType();

				if (actualExceptionType != expectedExceptionType)
					throw;
					//Assert.Fail("An exception of type {0} was expected, but an exception of type {1} was thrown with message: {2}", expectedExceptionType.Name, actualExceptionType.Name, exc.Message);

				assert((T)exc);
			}
		}

		public static Action<ScenarioAssertionException> Expect(string description, int lineNumber, string actual, string expected)
		{
			return delegate(ScenarioAssertionException exc)
			{
				Assert.That(exc.Description, Is.EqualTo(description));
				Assert.That(exc.LineIndex, Is.EqualTo(lineNumber - 1), "Line Index should be the Line Number - 1");
				Assert.That(exc.Actual, Is.EqualTo(actual));
				Assert.That(exc.Expected, Is.EqualTo(expected));
			};
		}

		public static Action<ScenarioAssertionException> Expect(string description, int lineNumber, string actual, Func<string, bool> assertExpected)
		{
			return delegate(ScenarioAssertionException exc)
			{
				Assert.That(exc.Description, Is.EqualTo(description));
				Assert.That(exc.LineIndex, Is.EqualTo(lineNumber - 1), "Line Index should be the Line Number - 1");
				Assert.That(exc.Actual, Is.EqualTo(actual));
				Assert.That(assertExpected(exc.Expected), Is.True);
			};
		}
	}
}