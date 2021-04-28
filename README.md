[![Build status](https://ci.appveyor.com/api/projects/status/hyaelibhdbeja6em?svg=true)](https://ci.appveyor.com/project/monkog/package-dependency-validator)

# Package Dependency Validator

This application analyzes the input file containing a description of software package dependencies.

## Usage

In order to validate the description of software package dependencies described in a test file follow the instruction:

1. Run NuGet restore
2. Build the application
3. Run in the console `PackageDependencyValidator.exe` followed by the test file name:

```
PackageDependencyValidator.exe input000.txt
```

4. The result will be printed in the console. It will state `PASS` if the package setup is correct and `FAIL` if the package setup is invalid.

## Tests

An AppVeyor configuration was added to this project. There is an integration test project set up, which tests the application with all provided test files. You can as well check the results [here](https://ci.appveyor.com/project/monkog/package-dependency-validator/build/tests)