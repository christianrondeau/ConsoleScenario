using System.Linq;

namespace ConsoleScenario.TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var testName = args.FirstOrDefault();

			new TestRunner(Tests.All()).Run(testName);
		}
	}
}
