using System.Linq;
using ConsoleScenario.Assertions;

namespace ConsoleScenario
{
	public static class ScenarioExtensions
	{
		public static IScenario Expect(this IScenario scenario, params string[] lines)
		{
			scenario.AddAssertions(lines.Select(LineEqualsAssertion.Create));
			return scenario;
		}

		public static IScenario IgnoreRemaining(this IScenario scenario)
		{
			scenario.AddAssertion(new IgnoreRemainingLinesAssertion());
			return scenario;
		}
	}
}