using System.IO;

namespace ConsoleScenario.StreamHandling
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
}