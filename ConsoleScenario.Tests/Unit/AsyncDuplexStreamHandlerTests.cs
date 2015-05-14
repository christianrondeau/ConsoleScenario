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
	public class AsyncDuplexStreamHandlerTests
	{
		public class CanWriteAndReadChars
		{
			private StringBuilder _sb;
			private AsyncDuplexStreamHandler _handler;

			[SetUp]
			public void WhenWritingChars()
			{
				_sb = new StringBuilder();
				_handler = new AsyncDuplexStreamHandler(
					new StreamReader(new EchoStream()),
					new StringWriter(_sb),
					new StringReader("")
					);

				_handler.Write("123");
				_handler.Write("456");
			}

			[Test]
			public void CharsAreWrittenToInput()
			{
				Assert.That(_sb.ToString(), Is.EqualTo("123456"));
			}

			[Test]
			public void CharsAreWrittenToOutput()
			{
				var timeout = TimeSpan.FromMilliseconds(1);
				Assert.That(_handler.Read(timeout), Is.EqualTo('1'));
				Assert.That(_handler.Read(timeout), Is.EqualTo('2'));
				Assert.That(_handler.Read(timeout), Is.EqualTo('3'));
				Assert.That(_handler.Read(timeout), Is.EqualTo('4'));
				Assert.That(_handler.Read(timeout), Is.EqualTo('5'));
				Assert.That(_handler.Read(timeout), Is.EqualTo('6'));
			}
		}

		public class CanWriteAndReadLines
		{
			private StringBuilder _sb;
			private AsyncDuplexStreamHandler _handler;

			[SetUp]
			public void WhenWritingLines()
			{
				_sb = new StringBuilder();
				_handler = new AsyncDuplexStreamHandler(
					new StreamReader(new EchoStream()),
					new StringWriter(_sb),
					new StringReader("")
					);

				_handler.WriteLine("Should end up in the StreamWriter");
				_handler.WriteLine("With a line break after each");
			}

			[Test]
			public void LinesAreWrittenToInput()
			{
				var expected = String.Join(Environment.NewLine, "Should end up in the StreamWriter", "With a line break after each", "");
				Assert.That(_sb.ToString(), Is.EqualTo(expected));
			}

			[Test]
			public void LinesAreWrittenToOutput()
			{
				var timeout = TimeSpan.FromMilliseconds(1);
				Assert.That(_handler.ReadLine(timeout), Is.EqualTo("Should end up in the StreamWriter"));
				Assert.That(_handler.ReadLine(timeout), Is.EqualTo("With a line break after each"));
			}
		}

		[Test]
		public void CanReadLinesUntilEndOfStream()
		{
			var sr = new StringReader(String.Join(
				Environment.NewLine,
				"Line 1",
				"Line 2"
				));

			var handler = new AsyncDuplexStreamHandler(sr, new StringWriter(), new StringReader(""));

			Assert.That(handler.ReadLine(TimeSpan.Zero), Is.EqualTo("Line 1"));
			Assert.That(handler.ReadLine(TimeSpan.Zero), Is.EqualTo("Line 2"));
			Assert.That(handler.ReadLine(TimeSpan.Zero), Is.Null);
		}

		[Test]
		[ExpectedException(typeof(TimeoutException), ExpectedMessage = "No result in alloted time: 00.1000s")]
		public void ThrowsWhenTimeoutReached()
		{
			var handler = new AsyncDuplexStreamHandler(
				new StreamReader(new BlockingStream()),
				new StringWriter(),
				new StringReader("")
				);

			handler.ReadLine(TimeSpan.FromMilliseconds(100));
		}

		public class CanWaitForExit
		{
			[Test]
			public void WaitForExitExitsImmediatelyWhenNothingToWaitFor()
			{
				var handler = new AsyncDuplexStreamHandler(
					new StringReader(""),
					new StringWriter(),
					new StringReader("")
					);

				handler.WaitForExit();
				Assert.Pass("Successfully skipped waiting since there was nothing to do");
			}

			[Test]
			public void WaitForExitBlocksWhenStillReading()
			{
				var blockingStream = new BlockingStream();

				var handler = new AsyncDuplexStreamHandler(
					new StreamReader(blockingStream),
					new StringWriter(),
					new StringReader("")
					);

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
}