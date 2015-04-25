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
					Assert.Fail("An exception of type {0} was expected, but am exception of type {1} was thrown", expectedExceptionType.Name, actualExceptionType.Name);

				assert((T)exc);
			}
		}

		public static Action<ScenarioAssertionException> Expect(string description, string actual, string expected)
		{
			return delegate(ScenarioAssertionException exc)
			{
				Assert.That(exc.Description, Is.EqualTo(description));
				Assert.That(exc.Actual, Is.EqualTo(actual));
				Assert.That(exc.Expected, Is.EqualTo(expected));
			};
		}
	}
}