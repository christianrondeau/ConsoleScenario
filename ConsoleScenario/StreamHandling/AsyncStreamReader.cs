using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleScenario.StreamHandling
{
	public sealed class AsyncStreamReader : IDisposable
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

			_task = new Task(ReadCharsAsync, _cancel.Token);
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

		private void ReadCharsAsync()
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
			_cancel.Cancel();
			_task.Wait();

			_task.Dispose();
			_cancel.Dispose();
			_pendingChars.Dispose();
		}
	}
}