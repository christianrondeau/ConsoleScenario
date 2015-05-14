using System;

namespace ConsoleScenario.Steps
{
	public interface IReadAssertionStep : IScenarioStep
	{
		IReadAssertionStep WithTimeout(TimeSpan timeout);
		IReadAssertionStep Times(int times);
	}
}