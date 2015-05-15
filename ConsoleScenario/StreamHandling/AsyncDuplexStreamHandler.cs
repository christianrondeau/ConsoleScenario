using System;
using System.IO;

namespace ConsoleScenario.StreamHandling
{
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
}
