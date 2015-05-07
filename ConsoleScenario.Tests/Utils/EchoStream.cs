﻿using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace ConsoleScenario.Tests.Utils
{
	public class EchoStream : MemoryStream
	{
		private readonly ManualResetEvent _dataReady = new ManualResetEvent(false);
		private readonly ConcurrentQueue<byte[]> _buffers = new ConcurrentQueue<byte[]>();

		public bool DataAvailable { get { return !_buffers.IsEmpty; } }

		public override void Write(byte[] buffer, int offset, int count)
		{
			_buffers.Enqueue(buffer);
			_dataReady.Set();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			_dataReady.WaitOne();

			byte[] lBuffer;

			if (!_buffers.TryDequeue(out lBuffer))
			{
				_dataReady.Reset();
				return -1;
			}

			if (!DataAvailable)
				_dataReady.Reset();

			Array.Copy(lBuffer, buffer, lBuffer.Length);
			return lBuffer.Length;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				_dataReady.Dispose();

			base.Dispose(disposing);
		}
	}
}
