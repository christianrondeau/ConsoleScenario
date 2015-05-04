using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ConsoleScenario
{
	public interface IProcessRuntime : IDisposable
	{
		TextReader StandardOutput { get; }
		TextWriter StandardInput { get; }
		bool ForceExit(TimeSpan? waitForExit);
	}

	public sealed class ProcessRuntime : IProcessRuntime
	{
		private const int ForceExitRetries = 3;
		private const int SleepBetweenRetriesMilliseconds = 100;
		private const int DefaultWaitForExitMilliseconds = 1000;

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

			try
			{
				_process.CloseMainWindow();
			}
			catch (InvalidOperationException)
			{
#if(DEBUG)
				Debug.WriteLine("DEBUG: CloseMainWindow failed on SUT process");
#endif
				if (_process.HasExited) return false;
			}

			if (_process.WaitForExit(waitForExit.HasValue ? (int) waitForExit.Value.TotalMilliseconds : DefaultWaitForExitMilliseconds))
				return false;

#if(DEBUG)
			Debug.WriteLine("DEBUG: WaitForExit failed on SUT process; Killing process");
#endif

			var exceptions = new List<Exception>();
			for (var retry = 0; retry < ForceExitRetries; retry++)
			{
				if (retry != 0)
					Thread.Sleep(SleepBetweenRetriesMilliseconds);

				try
				{
					_process.Kill();
					return true;
				}
				catch (Exception exc)
				{
					exceptions.Add(exc);
				}
			}

			throw new AggregateException("Unable to close nor kill the process", exceptions);
		}

		public void Dispose()
		{
			_process.Dispose();
		}
	}
}