using System.IO;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Unit
{
	[TestFixture]
	public class AsyncDuplexStreamHandlerFactoryTests
	{
		[Test]
		public void CanCreateAnInstance()
		{
			var output = new StringReader("");
			var input = new StringWriter();
			var error = new StringReader("");

			Assert.That(new AsyncDuplexStreamHandlerFactory().Create(output, input, error), Is.TypeOf<AsyncDuplexStreamHandler>());
		}
	}
}