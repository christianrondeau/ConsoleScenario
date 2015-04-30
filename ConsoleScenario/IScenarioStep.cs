namespace ConsoleScenario
{
	public interface IScenarioStep
	{
		void Run(IAsyncDuplexStreamHandler asyncTwoWayStreamsHandler, ref int lineIndex);
	}
}