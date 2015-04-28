using System;
using System.Diagnostics;
using System.IO;

namespace ConsoleScenario
{
	public class ProcessRuntime : IProcessRuntime
	{
		public static ProcessRuntime Start(string appPath, string args)
		{
			return new ProcessRuntime(new Process
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
			});
		}

		private readonly Process _process;

		public TextReader StandardOutput
		{
			get { return _process.StandardOutput; }
		}

		public TextWriter StandardInput
		{
			get { return _process.StandardInput; }
		}

		private ProcessRuntime(Process process)
		{
			if (process == null) throw new ArgumentNullException("process");

			_process = process;

			_process.Start();
		}

		public bool ForceExit(TimeSpan? waitForExit)
		{
			if (_process.HasExited) return false;

			_process.CloseMainWindow();

			if (_process.WaitForExit(
				waitForExit.HasValue
					? (int) waitForExit.Value.TotalMilliseconds
					: (int) ConsoleScenarioDefaults.Timeout.TotalSeconds))
				return false;

			_process.Kill();
			return true;
		}

		public void Dispose()
		{
			_process.Dispose();
		}
	}

	public interface IProcessRuntime : IDisposable
	{
		TextReader StandardOutput { get; }
		TextWriter StandardInput { get; }
		bool ForceExit(TimeSpan? waitForExit);
	}

	public class ProcessFactory : IProcessFactory
	{
		private readonly string _appPath;
		private readonly string _args;

		public ProcessFactory(string appPath, string args)
		{
			if (appPath == null) throw new ArgumentNullException("appPath");
			if (args == null) throw new ArgumentNullException("args");

			_appPath = appPath;
			_args = args;
		}

		public IProcessRuntime Start()
		{
			return ProcessRuntime.Start(_appPath, _args);
		}
	}

	public interface IProcessFactory
	{
		IProcessRuntime Start();
	}
}