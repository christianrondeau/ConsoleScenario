# ConsoleScenario

An automated test fixture for .NET console applications

[![AppVeyor Build Status](https://ci.appveyor.com/api/projects/status/ik4jo7xeia9xnada?svg=true)](https://ci.appveyor.com/project/christianrondeau/consolescenario)
[![Join the chat at https://gitter.im/christianrondeau/ConsoleScenario](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/christianrondeau/ConsoleScenario?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## Overview

Using ConsoleScenario, you can run a program, define a list of expectations and input so that your tests verify both the behavior and the output.

```csharp
Scenarios.Create("myapp.exe", "-argument")
  .Expect("Welcome to my app!")
  .ExpectPrompt("Do you want to continue? (y/n)")
  .Input("y")
  .Expect("You selected 'yes'")
  .Run(),
```

## Fluent API

These are all the expectations and console interactions supported by ConsoleScenario. You can also create your own assertions and steps.

Most functions also support a `TimeSpan` parameter to define a timeout, which will kill the process.

Most expectations will fail if the error stream contains something.

### Basic

* `.Expect("This line should be present")` insures that the next line is exactly this
* `.Expect("Multiple", "Lines", "At", "Once")` does the same thing as several back-to-back `Expect` calls
* `.Expect(line => line.Contains("something"))` allows providing a callback, receiving the line and returning whether the assertion is true
* `.Any(5)` ignores the line content, as long as the console indeed returns a line

### Prompts and input

* `.ExpectPrompt("Enter your name:")` is similar to `Expect`, but won't wait for the line break
* `.Input("John Doe")` sends the characters to the console

### Remaining and exit

* `.ExpectNothingElse()` will fail if the console outputs more lines
* `.IgnoreRemaining()` will ignore everything the console outputs until it closes
* `.ExpectExitCode(-1)` will fail if the console exit code is not the provided one
* `.IgnoreExitCode()` will not verify the exit code

### Error expectations

* `.ExpectError("Input string invalid")` will pass if the error stream contains that string

### Waiting

* `.Until(line => line.Contains("100%"))` will check every line until the callback returns true

### Extract and late bound

* `.Extract("Job ID: (.+)", values => jobId = values[0])` is a shortcut to run a regex and provide the values in the callback
* `.Expect(() => "Starting job " + jobId)` is the same as `Expect` but allows resolving the string at runtime, allowing the usage of `Extract` variables

## Extensibility

The general principle is that the console `Input`, `Output` and `Error` streams are being managed by the `ProcessRuntime` object, which is created and inject in `Scenario` by the `Scenarios` class for you.

The `Scenario` class simply executes `IScenarioStep` instances one by one until none are left.

### The `Scenario` class

The `Scenario` class is the starting point to running assertions.

You can set the `ExpectedExitCode` property to null to ignore the exit code, or to your expected exit code.

Add steps using `AddStep(step)` and/or `AddSteps(steps)`.

Execute `Run()` when you are ready to run all steps.

### Using the existing steps

* `ReadLineAssertionStep` will read a line, run it against an `IAssertion` and repeat if specified. Most expectations use this.
* `ReadUntilStep` will read lines until the specified condition is true
* `InputStep` will write the specified line in the `Input` stream
* `ReadCharsStep` will read characters until the provided string has been reached
* `ReadErrorLineAssertionStep` is similar to `ReadLineAssertionStep`, but checks in the `Error` stream

### Implementing your own `IAssertion`

You can implement your own `IAssertion`, and add it to the `Scenario` steps.

Once you have implemented your assertion, call `scenario.AddStep(new ReadLineAssertion(myAssertion))` where `myAssertion` is your assertion instance.

You can return either `AssertionResult.Pass()` or `AssertionResult.Fail("Message", "The expected value")`

For the fluent API, you can create your own `ScenarioExtensions` class, like this:

```csharp
public static class ScenarioExtensions
{
	public static IScenario Input(this IScenario scenario)
	{
		scenario.AddStep(new ReadLineAssertionStep(new MyAssertion()));
		return scenario;
	}
}
```

### Implementing your own `IScenarioStep`

If you need very advanced control of the flow of the assertion, you can implement your own `IScenarioStep`.

You have one function to implement, `void Run(IAsyncDuplexStreamHandler asyncDuplexStreamHandler, ref int lineIndex);`.

In your implementation, you should read from `asyncDuplexStreamHandler` as necessary, and increment `lineIndex` every time a line is read.

If your assertion fails, you can throw a new `ScenarioAssertionException`.

## License

Copyright (c) 2015 Christian Rondeau, [MIT License](LICENSE)
