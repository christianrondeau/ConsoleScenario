using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleScenario.Tests.Utils;
using NUnit.Framework;

namespace ConsoleScenario.Tests.Unit
{
	[TestFixture]
	public class AsyncDuplexStreamHandlerTests
	{
		[Test]
		public void CanWrite()
		{
			var sr = new StreamReader(new MemoryStream());
			var sb = new StringBuilder();
			var sw = new StringWriter(sb);

			var handler = new AsyncDuplexStreamHandler(sr, sw);

			handler.WriteLine("Should end up in the StreamWriter");
			handler.WriteLine("With a line break after each");

			Assert.That(sb.ToString(), Is.EqualTo("Should end up in the StreamWriter" + Environment.NewLine + "With a line break after each" + Environment.NewLine));
		}

		[Test]
		public void CanReadASingleLine()
		{
			var sr = new StringReader(String.Join(
				Environment.NewLine,
				"Line 1",
				"Line 2"
				));
			var sw = new StringWriter();

			var handler = new AsyncDuplexStreamHandler(sr, sw);

			Assert.That(handler.ReadLine(0), Is.EqualTo("Line 1"));
			Assert.That(handler.ReadLine(0), Is.EqualTo("Line 2"));
		}

		[Test]
		[ExpectedException(typeof(TimeoutException), ExpectedMessage = "No result in alloted time: 00.1000s")]
		public void CanReadUntilTimeout()
		{
			var stream = new BlockingStream();
			var sr = new StreamReader(stream);
			var sw = new StringWriter();

			var handler = new AsyncDuplexStreamHandler(sr, sw);

			handler.ReadLine(0.1);
		}

		[Test]
		public void CanReadUntilStreamComplete()
		{
			var sr = new StringReader("");
			var sw = new StringWriter();

			var handler = new AsyncDuplexStreamHandler(sr, sw);

			handler.ReadLine(0.1);
			Assert.That(handler.ReadLine(0.1), Is.Null);
		}

		[Test]
		public void WaitForExitExitsImmediatelyWhenNothingToWaitFor()
		{
			var sr = new StringReader("");
			var sw = new StringWriter();

			var handler = new AsyncDuplexStreamHandler(sr, sw);

			handler.WaitForExit();
			Assert.Pass("Successfully skipped waiting since there was nothing to do");
		}

		[Test]
		public void WaitForExitBlocksWhenStillReading()
		{
			var blockingStream = new BlockingStream();
			var sr = new StreamReader(blockingStream);
			var sw = new StringWriter();

			var handler = new AsyncDuplexStreamHandler(sr, sw);

			var stopWatch = new Stopwatch();
			stopWatch.Start();

			new Task(() =>
			{
				Thread.Sleep(100);
				blockingStream.Unblock();
			}).Start();
			handler.WaitForExit();

			stopWatch.Stop();

			if (stopWatch.ElapsedMilliseconds < 100)
				Assert.Fail("WaitForExit returned before the timer finished");
			else
				Assert.Pass("Successfully waited for the stream to end");
		}
	}
}