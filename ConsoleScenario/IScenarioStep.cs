using ConsoleScenario.StreamHandling;

namespace ConsoleScenario
{
	public interface IScenarioStep
	{
		void Run(IAsyncDuplexStreamHandler asyncDuplexStreamHandler, ref int lineIndex);
	}
}