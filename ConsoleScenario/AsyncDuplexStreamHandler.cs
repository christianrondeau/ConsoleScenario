using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
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
		char Read(TimeSpan timeout);
		string ReadLine(TimeSpan timeout);
		void WriteLine(string command);
		void WaitForExit();
	}

	public sealed class AsyncDuplexStreamHandler : IAsyncDuplexStreamHandler
	{
		private static readonly TimeSpan VeryLongTimeout = TimeSpan.FromDays(7);

		private readonly TextWriter _input;
		private readonly TextReader _output;
		private readonly CancellationTokenSource _cancel;
		private readonly Task _task;
		private readonly BlockingCollection<char> _pendingChars;

		public AsyncDuplexStreamHandler(TextReader output, TextWriter input)
		{
			_input = input;
			_output = output;

			_pendingChars = new BlockingCollection<char>();

			_cancel = new CancellationTokenSource();

			_task = new Task(ReadLinesAsync, _cancel.Token);
			_task.Start();
		}

		public char Read(TimeSpan timeout)
		{
			char c;

			if (timeout == TimeSpan.Zero)
				timeout = VeryLongTimeout;

			if (_pendingChars.TryTake(out c, timeout))
				return c;

			if (_pendingChars.IsAddingCompleted)
				return char.MinValue;

			throw new TimeoutException(string.Format(@"No result in alloted time: {0:ss\.ffff}s", timeout));
		}

		public string ReadLine(TimeSpan timeout)
		{
			var sb = new StringBuilder();

			char c;
			while ((c = Read(timeout)) != char.MinValue)
			{
				if (c == '\n')
					return sb.ToString();

				sb.Append(c);
			}

			return sb.Length == 0 ? null : sb.ToString();
		}

		public void Write(string command)
		{
			foreach (var c in command)
				_pendingChars.Add(c);

			_input.Write(command);
		}

		public void WriteLine(string command)
		{
			foreach (var c in command)
				_pendingChars.Add(c);

			_pendingChars.Add('\n');

			_input.WriteLine(command);
		}

		public void WaitForExit()
		{
			_task.Wait();
		}

		public void Dispose()
		{
			if (_task != null)
			{
				_cancel.Cancel();
				_task.Wait();
				_task.Dispose();
			}

			if (_pendingChars != null)
			{
				_pendingChars.Dispose();
			}
		}

		private void ReadLinesAsync()
		{
#if(DEBUG)
			var output = new StringBuilder();
#endif
			int nextChar;
			while ((nextChar = _output.Read()) != -1)
			{
				if (_cancel.IsCancellationRequested)
				{
					_pendingChars.CompleteAdding();
					return;
				}

				var value = (char) nextChar;

				if (nextChar == '\r')
					continue;

#if(DEBUG)
				if (value == '\n')
				{
					Debug.WriteLine("DEBUG: " + output);
					output.Clear();
				}
				else
				{
					output.Append(value);
				}
#endif

				_pendingChars.Add(value);
			}

#if(DEBUG)
			if (output.Length > 0)
				Debug.WriteLine("DEBUG: " + output);

			Debug.WriteLine("DEBUG: >> End of stream");
#endif

			_pendingChars.CompleteAdding();
		}
	}
}
