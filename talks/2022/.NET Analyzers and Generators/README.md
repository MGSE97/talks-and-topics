[< Back to talks](../README.md)

---

# .NET Analyzers and Generators

This shows example how we can create custom Analyzers and Generators since .NET 5.
More information can be found in official documentation ([docs](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/), [tutorial](https://learn.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/tutorials/how-to-write-csharp-analyzer-code-fix))

## Running example

1. Open `examples/AnalyzersAndGenerators.sln` in Visual Studio 2019+
2. Run `Analyzer.Vsix` project
3. New VS Instance will open
4. Open same project
5. Navigate to `Test/Api/Endpoints`
6. Open `Hello*Endpoint.cs`
7. Wait for analyzers to run
8. You should see custom errors, warnings, info and code fixes for them.

## Analyzers

Analyzers will go through code, and find out code issues. You can publish your custom check for code. Everybody, who wants to use them, needs to install `vsix` package!

Analyzer code is in `Analyzer/Analyzer/EndpointAnalyzer.`. If you follow comments there, you should understand what is it doing.

## Code fixes

Code fixes are proposed changes to change your code, based on analyzer errors, warnings, info, etc.

They are located in `Analyzer/Analyzer.CodeFixes`. Each code fix has its own file.
`EndpointCodeFixProvider` registers code fixes for each diagnostics id.

## Generators

Generators will create code files based on existing project code.

This part was not done, since talk was canceled.

## Authors

- [**MGSE**](https://github.com/MGSE97)

---

<h6 align="center">

• &nbsp; [T&T](../../../README.md) &nbsp;
•>&nbsp; Talks &nbsp;<•
&nbsp; [Topics](../../../topics/README.md) &nbsp;
•

</h6>

<h6 align="center">

•>&nbsp; 2022 &nbsp;<•
&nbsp; [2023](../../2023/README.md) &nbsp;
•

</h6>

<h6 align="center">

[< Back to talks](../README.md)
&nbsp;&nbsp; • &nbsp;&nbsp;
<b><a href="https://github.com/MGSE97" target="_blank">MGSE</a> ☕ 2016 ... 2023</b>

</h6>
