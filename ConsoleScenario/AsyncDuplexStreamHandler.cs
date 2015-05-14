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
		IAsyncDuplexStreamHandler Create(TextReader output, TextWriter input, TextReader error);
	}

	public class AsyncDuplexStreamHandlerFactory : IAsyncDuplexStreamHandlerFactory
	{
		public IAsyncDuplexStreamHandler Create(TextReader output, TextWriter input, TextReader error)
		{
			return new AsyncDuplexStreamHandler(output, input, error);
		}
	}

	public interface IAsyncDuplexStreamHandler : IDisposable
	{
		char Read(TimeSpan timeout);
		string ReadLine(TimeSpan timeout);
		string ReadError(TimeSpan timeout);
		void WriteLine(string command);
		void WaitForExit();
	}

	public sealed class AsyncDuplexStreamHandler : IAsyncDuplexStreamHandler
	{
		private readonly TextWriter _input;

		private readonly AsyncStreamReader _outputReader;
		private readonly AsyncStreamReader _errorReader;

		public AsyncDuplexStreamHandler(TextReader output, TextWriter input, TextReader error)
		{
			_input = input;

			_outputReader = new AsyncStreamReader(output, "out");
			_errorReader = new AsyncStreamReader(error, "err");
		}

		public char Read(TimeSpan timeout)
		{
			CrashIfErrorPending(timeout);

			return _outputReader.Read(timeout);
		}

		public string ReadLine(TimeSpan timeout)
		{
			CrashIfErrorPending(timeout);

			return _outputReader.ReadLine(timeout);
		}

		public string ReadError(TimeSpan timeout)
		{
			var errorLine = _errorReader.ReadLine(timeout);

			if (String.IsNullOrWhiteSpace(errorLine))
				// When throwing an exception, the first line will be empty
				errorLine = _errorReader.ReadLine(timeout);

			return errorLine;
		}

		private void CrashIfErrorPending(TimeSpan timeout)
		{
			if (!_errorReader.Pending()) return;

			var errorLine = ReadError(timeout);

			throw new ApplicationException(errorLine);
		}

		public void Write(string command)
		{
			_outputReader.Write(command);
			_input.Write(command);
		}

		public void WriteLine(string command)
		{
			_outputReader.WriteLine(command);
			_input.WriteLine(command);
		}

		public void WaitForExit()
		{
			_outputReader.Wait();
			_errorReader.Wait();
		}

		public void Dispose()
		{
			_outputReader.Dispose();
			_errorReader.Dispose();
		}
	}

	public class AsyncStreamReader : IDisposable
	{
		private static readonly TimeSpan VeryLongTimeout = TimeSpan.FromDays(7);

		private readonly string _name;
		private readonly TextReader _stream;
		private readonly CancellationTokenSource _cancel;
		private readonly Task _task;
		private readonly BlockingCollection<char> _pendingChars;

		public AsyncStreamReader(TextReader stream, string name)
		{
			if (stream == null) throw new ArgumentNullException("stream");
			if (name == null) throw new ArgumentNullException("name");

			_stream = stream;
			_name = name;

			_pendingChars = new BlockingCollection<char>();

			_cancel = new CancellationTokenSource();

			_task = new Task(ReadLinesAsync, _cancel.Token);
			_task.Start();
		}

		public bool Pending()
		{
			return _pendingChars.Count > 0;
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
		}

		public void WriteLine(string command)
		{
			Write(command);
			_pendingChars.Add('\n');
		}

		private void ReadLinesAsync()
		{
#if(DEBUG)
			var output = new StringBuilder();
#endif
			int nextChar;
			while ((nextChar = _stream.Read()) != -1)
			{
				if (_cancel.IsCancellationRequested)
				{
					_pendingChars.CompleteAdding();
					return;
				}

				var value = (char)nextChar;

				if (nextChar == '\r')
					continue;

#if(DEBUG)
				if (value == '\n')
				{
					Debug.WriteLine("DEBUG ({0}): {1}", _name, output);
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
				Debug.WriteLine("DEBUG ({0}): {1}", _name, output);

			Debug.WriteLine(string.Format("DEBUG ({0}): >> End of stream", _name));
#endif

			_pendingChars.CompleteAdding();
		}

		public void Wait()
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
				_pendingChars.Dispose();
		}
	}
}
