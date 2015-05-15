using System;
using System.Collections.Generic;
using ConsoleScenario.StreamHandling;

namespace ConsoleScenario
{
	public interface IScenario
	{
		void Run(TimeSpan? waitForExit = null);
		void AddStep(IScenarioStep step);
		void AddSteps(IEnumerable<IScenarioStep> steps);
	}

	public class Scenario : IScenario
	{
		private readonly List<IScenarioStep> _steps;
		private readonly IProcessRuntimeFactory _processRuntimeFactory;
		private readonly IAsyncDuplexStreamHandlerFactory _asyncDuplexStreamHandlerFactory;

		public Scenario(IProcessRuntimeFactory processRuntimeFactory, IAsyncDuplexStreamHandlerFactory asyncDuplexStreamHandlerFactory)
		{
			if (processRuntimeFactory == null) throw new ArgumentNullException("processRuntimeFactory");
			if (asyncDuplexStreamHandlerFactory == null) throw new ArgumentNullException("asyncDuplexStreamHandlerFactory");

			_processRuntimeFactory = processRuntimeFactory;
			_asyncDuplexStreamHandlerFactory = asyncDuplexStreamHandlerFactory;

			_steps = new List<IScenarioStep>();
		}

		public void AddStep(IScenarioStep step)
		{
			_steps.Add(step);
		}

		public void AddSteps(IEnumerable<IScenarioStep> steps)
		{
			_steps.AddRange(steps);
		}

		public void Run(TimeSpan? waitForExit = null)
		{
			using (var process = _processRuntimeFactory.Start())
			{
				var lineIndex = -1;
				Exception exception = null;
				try
				{
					var asyncDuplexStreamHandler = _asyncDuplexStreamHandlerFactory.Create(
											process.StandardOutput,
											process.StandardInput,
											process.StandardError
											);

					RunSteps(asyncDuplexStreamHandler, ref lineIndex);
				}
				catch (Exception exc)
				{
					exception = exc;
					throw;
				}
				finally
				{
					if (process.ForceExit(waitForExit) && exception == null)
					{
						var lineThatNeverCameIndex = lineIndex + 1;
						throw new ScenarioAssertionException("Process wait for exit timeout", lineThatNeverCameIndex, null, null);
					}
				}
			}
		}

		private void RunSteps(IAsyncDuplexStreamHandler asyncDuplexStreamHandler, ref int lineIndex)
		{
			var stepsEnumerator = _steps.GetEnumerator();

			while (stepsEnumerator.MoveNext())
			{
				var step = stepsEnumerator.Current;
				if (step == null) continue;

				step.Run(asyncDuplexStreamHandler, ref lineIndex);
			}
		}
	}
}