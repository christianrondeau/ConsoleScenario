using System;
using System.Linq;
using System.Text.RegularExpressions;
using ConsoleScenario.Assertions;
using ConsoleScenario.Steps;

namespace ConsoleScenario
{
	public static class ScenarioExtensions
	{
		public static IScenario Input(this IScenario scenario, string value)
		{
			scenario.AddStep(new InputStep(value));
			scenario.AddStep(new ReadLineAssertionStep(new AnyLineAssertion())); // Read what was just inputted
			return scenario;
		}

		public static IScenario ExpectPrompt(this IScenario scenario, string value)
		{
			scenario.AddStep(new ReadCharsStep(value));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, string line, TimeSpan timeout)
		{
			scenario.AddStep(new ReadLineAssertionStep(new LineEqualsAssertion(line)).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, params string[] lines)
		{
			scenario.AddSteps(lines.Select(line => new ReadLineAssertionStep(new LineEqualsAssertion(line))));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, Func<string, bool> callback, TimeSpan timeout)
		{
			scenario.AddStep(new ReadLineAssertionStep(new CallbackAssertion(callback)).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, Func<string, bool> callback)
		{
			scenario.AddStep(new ReadLineAssertionStep(new CallbackAssertion(callback)));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, Func<string> expectedLineFn, TimeSpan timeout)
		{
			scenario.AddStep(new ReadLineAssertionStep(new LateLineEqualsAssertion(expectedLineFn)).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario Expect(this IScenario scenario, Func<string> expectedLineFn)
		{
			scenario.AddStep(new ReadLineAssertionStep(new LateLineEqualsAssertion(expectedLineFn)));
			return scenario;
		}

		public static IScenario Any(this IScenario scenario, int count, TimeSpan timeout)
		{
			scenario.AddStep(new ReadLineAssertionStep(new AnyLineAssertion()).Times(count).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario Any(this IScenario scenario, int count = 1)
		{
			scenario.AddStep(new ReadLineAssertionStep(new AnyLineAssertion()).Times(count));
			return scenario;
		}

		public static IScenario ExpectNothingElse(this IScenario scenario)
		{
			scenario.AddStep(new ReadLineAssertionStep(new NoExtraneousLinesAssertion()));
			return scenario;
		}

		public static IScenario ExpectNothingElse(this IScenario scenario, TimeSpan timeout)
		{
			scenario.AddStep(new ReadLineAssertionStep(new NoExtraneousLinesAssertion()).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario IgnoreRemaining(this IScenario scenario)
		{
			scenario.AddStep(new ReadLineAssertionStep(new IgnoreLineAssertion()).Times(Int32.MaxValue));
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

		public static IScenario Extract(this IScenario scenario, string pattern, Action<string[]> assign, TimeSpan timeout)
		{
			scenario.AddStep(new ReadLineAssertionStep(new CallbackAssertion(line => CreateExtractCallback(pattern, line, assign))).WithTimeout(timeout));
			return scenario;
		}

		public static IScenario Extract(this IScenario scenario, string pattern, Action<string[]> assign)
		{
			scenario.AddStep(new ReadLineAssertionStep(new CallbackAssertion(line => CreateExtractCallback(pattern, line, assign))));
			return scenario;
		}

		private static bool CreateExtractCallback(string pattern, string line, Action<string[]> assign)
		{
			var match = Regex.Match(line, pattern);

			if (!match.Success)
				return false;

			assign(match.Groups.Cast<Capture>().Skip(1).Select(group => group.Value).ToArray());
			return true;
		}
	}
}