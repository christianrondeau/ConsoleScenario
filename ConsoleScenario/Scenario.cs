using System;
using System.Diagnostics;
using System.Linq;

namespace ConsoleScenario
{
	public class Scenario
	{
		private readonly string _appPath;
		private readonly string _args;
		private string _expectedOutput;

		public Scenario(string appPath, string args = null)
		{
			if (appPath == null) throw new ArgumentNullException("appPath");

			_appPath = appPath;
			_args = args;
		}

		public Scenario Expect(string output)
		{
			_expectedOutput = output;
			return this;
		}

		public void Run()
		{
			using (var process = Process.Start(new ProcessStartInfo
				{
					FileName = _appPath,
					Arguments = _args,
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden,
					UseShellExecute = false,
					RedirectStandardInput = true,
					RedirectStandardError = true,
					RedirectStandardOutput = true
				}))
			{
				var asyncTwoWayStreamsHandler = new AsyncDuplexStreamsHandlerFactory().Create(process.StandardOutput, process.StandardInput);

				var output = asyncTwoWayStreamsHandler.ReadUntil(10, "");

				if (output != _expectedOutput)
					throw new Exception(string.Format("Invalid console output. Expected:{0}{1}{0}Received:{0}{2}", Environment.NewLine, _expectedOutput, output));
			}
		}
	}
}