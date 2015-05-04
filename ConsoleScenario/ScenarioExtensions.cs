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
			scenario.AddStep(new ReadStep(new LineEqualsAssertion(line)).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, params string[] lines)
		{
			scenario.AddSteps(lines.Select(line => new ReadStep(new LineEqualsAssertion(line))));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, Func<string, bool> callback, TimeSpan timeout)
		{
			scenario.AddStep(new ReadStep(new CallbackAssertion(callback)).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, Func<string, bool> callback)
		{
			scenario.AddStep(new ReadStep(new CallbackAssertion(callback)));
			return scenario;
		}

		public static IScenario Any(this IScenario scenario, int count, TimeSpan timeout)
		{
			scenario.AddStep(new ReadStep(new AnyLineAssertion()).Times(count).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario Any(this IScenario scenario, int count = 1)
		{
			scenario.AddStep(new ReadStep(new AnyLineAssertion()).Times(count));
			return scenario;
		}

		public static IScenario ExpectNothingElse(this IScenario scenario)
		{
			scenario.AddStep(new ReadStep(new NoExtraneousLinesAssertion()));
			return scenario;
		}

		public static IScenario ExpectNothingElse(this IScenario scenario, TimeSpan timeout)
		{
			scenario.AddStep(new ReadStep(new NoExtraneousLinesAssertion()).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario IgnoreRemaining(this IScenario scenario)
		{
			scenario.AddStep(new ReadStep(new IgnoreLineAssertion()).Times(Int32.MaxValue));
			return scenario;
		}

		public static IScenario Until(this IScenario scenario, Func<string, bool> condition)
		{
			scenario.AddStep(new ReadUntilStep(condition));
			return scenario;
		}
	}
}