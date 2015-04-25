using System.Linq;

namespace ConsoleScenario
{
	public static class ScenarioExtensions
	{
		public static IScenario Expect(this IScenario scenario, params string[] lines)
		{
			scenario.AddAssertions(lines.Select(LineEqualsAssertion.Create));
			return scenario;
		}
	}
}