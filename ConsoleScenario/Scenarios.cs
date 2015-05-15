using ConsoleScenario.StreamHandling;

namespace ConsoleScenario
{
	public class Scenarios
	{
		public static IScenario Create(string appPath, string args)
		{
			return new Scenario(
				new ProcessRuntimeFactory(appPath, args),
				new AsyncDuplexStreamHandlerFactory()
				);
		}
	}
}