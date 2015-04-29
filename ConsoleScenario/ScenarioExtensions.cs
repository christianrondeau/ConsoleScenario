using System;
using System.Linq;
using ConsoleScenario.Assertions;

namespace ConsoleScenario
{
	public static class ScenarioExtensions
	{
		public static IScenario Input(this IScenario scenario, string value)
		{
			scenario.AddStep(new ScenarioStep(new InputLineAssertion(value)));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, string line, TimeSpan timeout)
		{
			scenario.AddStep(new ScenarioStep(new LineEqualsAssertion(line)).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, params string[] lines)
		{
			scenario.AddSteps(lines.Select(line => new ScenarioStep(new LineEqualsAssertion(line))));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, Func<string, bool> callback, TimeSpan timeout)
		{
			scenario.AddStep(new ScenarioStep(new CallbackAssertion(callback)).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, Func<string, bool> callback)
		{
			scenario.AddStep(new ScenarioStep(new CallbackAssertion(callback)));
			return scenario;
		}

		public static IScenario Any(this IScenario scenario, int count, TimeSpan timeout)
		{
			scenario.AddStep(new ScenarioStep(new AnyLineAssertion(count)).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario Any(this IScenario scenario, int count = 1)
		{
			scenario.AddStep(new ScenarioStep(new AnyLineAssertion(count)));
			return scenario;
		}

		public static IScenario ExpectNothingElse(this IScenario scenario)
		{
			scenario.AddStep(new ScenarioStep(new NoExtraneousLinesAssertion()));
			return scenario;
		}

		public static IScenario ExpectNothingElse(this IScenario scenario, TimeSpan timeout)
		{
			scenario.AddStep(new ScenarioStep(new NoExtraneousLinesAssertion()).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario IgnoreRemaining(this IScenario scenario)
		{
			scenario.AddStep(new ScenarioStep(new IgnoreRemainingLinesAssertion()));
			return scenario;
		}
	}
}