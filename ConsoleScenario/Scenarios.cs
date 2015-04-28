namespace ConsoleScenario
{
	public class Scenarios
	{
		public static IScenario Create(string appPath, string args)
		{
			return new Scenario(new ProcessFactory(appPath, args), new AsyncDuplexStreamHandlerFactory());
		}
	}
}