# ConsoleScenario

An automated test fixture for .NET console applications

[![Join the chat at https://gitter.im/christianrondeau/ConsoleScenario](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/christianrondeau/ConsoleScenario?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## Under Development

This project is not yet ready for general use. Not even alpha. Not even "hey look at it!". More like "pile of unstable code attempts".

Here is an overview of the main features that should be implemented

- [X] Ability to expect a line of console output
- [ ] Ability to expect multiple lines of console output
- [ ] Validate if the console output is smaller or longer than expected
- [ ] Ability to input something in the console
- [ ] Ability to expect the console to close
- [ ] Ability to provide a regex instead of a string
- [ ] Ability to read a line of console output and place it in a variable, and then use it in later expects (e.g. as a callback per line)
- [ ] Ability to private custom line assertion callbacks
- [ ] Ensure Process is stopped if an assertion error is thrown, or if the Scenario is disposed
- [ ] Determine whether "remaining" unasserted output is accepted or not

## License

Copyright (c) 2015 Christian Rondeau, [MIT License](LICENSE)