using System;
using System.Linq;
using ConsoleScenario.Assertions;
using ConsoleScenario.Steps;

namespace ConsoleScenario
{
	public static class ScenarioExtensions
	{
		public static IScenario Input(this IScenario scenario, string value)
		{
			scenario.AddStep(new InputStep(value));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, string line, TimeSpan timeout)
		{
			scenario.AddStep(new ReadAssertionAssertionStep(new LineEqualsAssertion(line)).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, params string[] lines)
		{
			scenario.AddSteps(lines.Select(line => new ReadAssertionAssertionStep(new LineEqualsAssertion(line))));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, Func<string, bool> callback, TimeSpan timeout)
		{
			scenario.AddStep(new ReadAssertionAssertionStep(new CallbackAssertion(callback)).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, Func<string, bool> callback)
		{
			scenario.AddStep(new ReadAssertionAssertionStep(new CallbackAssertion(callback)));
			return scenario;
		}

		public static IScenario Any(this IScenario scenario, int count, TimeSpan timeout)
		{
			scenario.AddStep(new ReadAssertionAssertionStep(new AnyLineAssertion()).Times(count).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario Any(this IScenario scenario, int count = 1)
		{
			scenario.AddStep(new ReadAssertionAssertionStep(new AnyLineAssertion()).Times(count));
			return scenario;
		}

		public static IScenario ExpectNothingElse(this IScenario scenario)
		{
			scenario.AddStep(new ReadAssertionAssertionStep(new NoExtraneousLinesAssertion()));
			return scenario;
		}

		public static IScenario ExpectNothingElse(this IScenario scenario, TimeSpan timeout)
		{
			scenario.AddStep(new ReadAssertionAssertionStep(new NoExtraneousLinesAssertion()).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario IgnoreRemaining(this IScenario scenario)
		{
			scenario.AddStep(new ReadAssertionAssertionStep(new IgnoreLineAssertion()).Times(Int32.MaxValue));
			return scenario;
		}

		public static IScenario Until(this IScenario scenario, Func<string, bool> condition)
		{
			scenario.AddStep(new ReadUntilStep(condition));
			return scenario;
		}

		public static IScenario Until(this IScenario scenario, Func<string, bool> condition, TimeSpan timeout)
		{
			scenario.AddStep(new ReadUntilStep(condition).WithTimeout(timeout));
			return scenario;
		}
	}
}