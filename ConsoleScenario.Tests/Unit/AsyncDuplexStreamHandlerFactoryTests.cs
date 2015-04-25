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
			var sr = new StringReader("");
			var sw = new StringWriter();

			Assert.That(new AsyncDuplexStreamHandlerFactory().Create(sr, sw), Is.TypeOf<AsyncDuplexStreamHandler>());
		}
	}
}