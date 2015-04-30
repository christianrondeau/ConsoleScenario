using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleScenario
{
	public interface IAsyncDuplexStreamHandlerFactory
	{
		IAsyncDuplexStreamHandler Create(TextReader output, TextWriter input);
	}

	public class AsyncDuplexStreamHandlerFactory : IAsyncDuplexStreamHandlerFactory
	{
		public IAsyncDuplexStreamHandler Create(TextReader output, TextWriter input)
		{
			return new AsyncDuplexStreamHandler(output, input);
		}
	}

	public interface IAsyncDuplexStreamHandler : IDisposable
	{
		string ReadLine(TimeSpan timeout);
		void WriteLine(string command);
		void WaitForExit();
	}

	public class AsyncDuplexStreamHandler : IAsyncDuplexStreamHandler
	{
		private readonly TextWriter _input;
		private readonly TextReader _output;
		private readonly Task _task;
		private readonly BlockingCollection<string> _pendingOutputLines = new BlockingCollection<string>();

		public AsyncDuplexStreamHandler(TextReader output, TextWriter input)
		{
			_input = input;
			_output = output;

			_task = new Task(ReadLinesAsync);
			_task.Start();
		}

		public string ReadLine(TimeSpan timeout)
		{
			string line;
			
			if (timeout == TimeSpan.Zero)
				timeout = TimeSpan.FromDays(7);

			if (_pendingOutputLines.TryTake(out line, timeout))
				return line;

			if (_pendingOutputLines.IsAddingCompleted)
				return null;

			throw new TimeoutException(string.Format(@"No result in alloted time: {0:ss\.ffff}s", timeout));
		}

		public void WriteLine(string command)
		{
			_input.WriteLine(command);
		}

		public void WaitForExit()
		{
			_task.Wait();
		}

		public void Dispose()
		{
			if (_task != null) _task.Dispose();
		}

		private void ReadLinesAsync()
		{
			string line;
			while ((line = _output.ReadLine()) != null)
			{
#if(DEBUG)
				Debug.WriteLine("DEBUG: " + line);
#endif
				_pendingOutputLines.Add(line);
			}

#if(DEBUG)
			Debug.WriteLine("DEBUG: >> Out stream closed");
#endif
			_pendingOutputLines.CompleteAdding();
		}
	}
}
