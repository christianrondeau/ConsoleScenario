using System.Diagnostics;

namespace ConsoleScenario
{
	public class Scenarios
	{
		public static IScenario Create(string appPath, string args)
		{
			var process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = appPath,
					Arguments = args,
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden,
					UseShellExecute = false,
					RedirectStandardInput = true,
					RedirectStandardError = true,
					RedirectStandardOutput = true
				}
			};

			return new Scenario(process, new AsyncDuplexStreamHandlerFactory());
		}
	}
}