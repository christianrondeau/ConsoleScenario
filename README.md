# ConsoleScenario

An automated test fixture for .NET console applications

[![Join the chat at https://gitter.im/christianrondeau/ConsoleScenario](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/christianrondeau/ConsoleScenario?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## Under Development

This project is not yet ready for general use. Not even alpha. Not even "hey look at it!". More like "pile of unstable code attempts".

Here is an overview of the main features that should be implemented

- [X] Ability to expect a line of console output
- [X] Ability to expect multiple lines of console output
- [X] Validate if the console output is smaller or longer than expected
- [X] If the console closes earlier than expected, don't wait for another line (`AsyncDuplexStreamHandler`)
- [X] Determine whether "remaining" unasserted output is accepted or not
- [X] Kill the console if the test times out and the console does not close itself
- [X] Ability to input something in the console
- [X] Ability to expect any line(s) of text of console output
- [ ] Ability to provide a regex instead of a string
- [X] Ability to assert a line using a callback
- [ ] Ability to specify "until", e.g. "until a line contains 'COMPLETE'"
- [ ] Ability to input something in the console when there is a prompt
- [ ] Ability to wait until a specific line using a callback (especially for Any)
- [ ] Ability to read a line of console output and place it in a variable, and then use it in later expects (e.g. as a callback per line)
- [ ] Ability to private custom line assertion callbacks
- [ ] Ensure Process is stopped if an assertion error is thrown, or if the Scenario is disposed
- [ ] Handle console errors and forward them as assertion failed
- [ ] Expect console errors
- [ ] Input and output characters instead of lines
- [ ] Nice documentation

### Other TODOs

- [ ] Refactor to avoid having two constructors for every `ScenarioExtensions` entry
- [ ] Refactor the `Scenario.AddStep` and instead use `AddAssertion` and `AddInput`
- [ ] Move IInput out of the Assertions folder
- [ ] Refactor the multi-line and single-line assertions to avoid duplication (e.g. `scenario.Any().Times(5)` where `.Times`, would simply wrap the assertion in a repeater. `.Once` would do nothing (readability) and `Until` would allow using any assertion)
- [ ] To allow `Until`, split the assertion "container" and the assertion code itself

## Example

```csharp
Scenarios.Create("myapp.exe", "-argument")
  .Expect(
    "Here is the console output",
    "That you expect"
  )
  .Expect("You can specify a timeout per line", TimeSpan.FromSeconds(0.5))
  .Expect(line => line.Contains("You can also specify callbacks"))
  .Any(3) // Or just skip a few lines
  .Input("You can input something too!")
  .ExpectNothingElse() // or .IgnoreRemaining()
  .Run(),
```

## License

Copyright (c) 2015 Christian Rondeau, [MIT License](LICENSE)