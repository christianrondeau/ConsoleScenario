using NUnit.Framework;

namespace ConsoleScenario.Tests.Functional
{
	public class ColorTests : EndToEndTestsBase
	{
		[Test]
		public void Success()
		{
			GivenATestConsoleScenario("color")
				.Expect("This test is green")
				.Expect("This test is red")
				.Run();
		}
	}
}