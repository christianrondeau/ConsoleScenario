using System;

namespace ConsoleScenario
{
	public interface IProcessRuntimeFactory
	{
		IProcessRuntime Start();
	}

	public class ProcessRuntimeFactory : IProcessRuntimeFactory
	{
		private readonly string _appPath;
		private readonly string _args;

		public ProcessRuntimeFactory(string appPath, string args)
		{
			if (appPath == null) throw new ArgumentNullException("appPath");
			if (args == null) throw new ArgumentNullException("args");

			_appPath = appPath;
			_args = args;
		}

		public IProcessRuntime Start()
		{
			return ProcessRuntime.Start(_appPath, _args);
		}
	}
}