using System;
using System.Collections.Generic;

namespace ConsoleScenario.TestApp
{
	public class TestRunner
	{
		private readonly IDictionary<string, Action> _testRepository;

		public TestRunner(IDictionary<string, Action> testRepository)
		{
			if (testRepository == null) throw new ArgumentNullException("testRepository");

			_testRepository = testRepository;
		}

		public void Run(string testName)
		{
			Action test;

			if (!_testRepository.TryGetValue(testName, out test))
				throw new ApplicationException(string.Format("Unknown test name: {0}", testName));

			test();
		}
	}
}