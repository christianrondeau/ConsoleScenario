using System;
using System.Linq;
using ConsoleScenario.Assertions;

namespace ConsoleScenario
{
	public static class ScenarioExtensions
	{
		public static IScenario Input(this IScenario scenario, string value)
		{
			scenario.AddAssertion(new InputLineAssertion(value));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, string line, TimeSpan timeout)
		{
			scenario.AddAssertion(new LineEqualsAssertion(line, timeout));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, params string[] lines)
		{
			scenario.AddAssertions(lines.Select(LineEqualsAssertion.Create));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, Func<string, bool> callback, TimeSpan timeout)
		{
			scenario.AddAssertion(new CallbackAssertion(callback, timeout));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, Func<string, bool> callback)
		{
			scenario.AddAssertion(new CallbackAssertion(callback));
			return scenario;
		}

		public static IScenario Any(this IScenario scenario, int count, TimeSpan timeout)
		{
			scenario.AddAssertion(new AnyLineAssertion(count, timeout));
			return scenario;
		}

		public static IScenario Any(this IScenario scenario, int count = 1)
		{
			scenario.AddAssertion(new AnyLineAssertion(count));
			return scenario;
		}

		public static IScenario ExpectNothingElse(this IScenario scenario)
		{
			scenario.AddAssertion(new NoExtraneousLinesAssertion());
			return scenario;
		}

		public static IScenario ExpectNothingElse(this IScenario scenario, TimeSpan timeout)
		{
			scenario.AddAssertion(new NoExtraneousLinesAssertion(timeout));
			return scenario;
		}

		public static IScenario IgnoreRemaining(this IScenario scenario)
		{
			scenario.AddAssertion(new IgnoreRemainingLinesAssertion());
			return scenario;
		}
	}
}