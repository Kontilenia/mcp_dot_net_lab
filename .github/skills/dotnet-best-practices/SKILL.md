---
name: dotnet-best-practices
description: "Write, modify, refactor, or review C# and .NET code using modern .NET best practices. Use whenever a task involves C#, .NET, ASP.NET Core, class libraries, console applications, workers, tests, project files, or solution files. Keep code simple, readable, maintainable, idiomatic, type-safe, testable, and appropriately documented."
user-invocable: true
disable-model-invocation: false
---

# .NET Best Practices

Apply these guidelines whenever working with C# or .NET code.

## Core Principles

1. Prefer the simplest complete solution that satisfies the requirement.
2. Optimize for correctness, clarity, maintainability, and predictable behavior.
3. Follow the repository's established architecture, naming, formatting, and
   conventions unless they conflict with correctness or the user's request.
4. Avoid speculative abstractions, unnecessary layers, premature optimization,
   clever code, and duplicated logic.
5. Keep changes focused. Do not refactor unrelated code.

## Readable and Simple Code

- Use descriptive names that communicate intent.
- Keep methods focused on one responsibility and at a readable size.
- Use early returns to reduce nesting when they improve clarity.
- Prefer straightforward control flow over dense expressions or long method
  chains.
- Extract shared logic only when it removes meaningful duplication or gives a
  concept a clear name.
- Prefer composition and dependency injection over hidden global state.
- Use language features supported by the repository's configured C# version.
- Preserve type safety; avoid `dynamic`, reflection, null-forgiving operators,
  and unsafe casts unless they are necessary and justified.

## Comments and Documentation

- Write code that explains itself through structure and naming.
- Add concise comments for non-obvious intent, constraints, business rules,
  workarounds, or reasoning.
- Explain why something is done, not what an obvious statement does.
- Do not add comments that merely repeat the code.
- Keep comments accurate when behavior changes.
- Add XML documentation to public APIs when the repository requires it or when
  consumers need behavior, parameter, exception, or return-value guidance.

## C# and .NET Practices

- Enable and respect nullable reference types where the project supports them.
- Validate inputs at system boundaries and use appropriate exceptions or
  result types consistently with the repository.
- Use specific exception types. Do not swallow exceptions or use broad catches
  unless adding context, logging, cleanup, or recovery.
- Accept and propagate `CancellationToken` in cancellable asynchronous work.
- Use `async` and `await` for asynchronous I/O; avoid blocking with `.Result`,
  `.Wait()`, or `GetAwaiter().GetResult()`.
- Do not use `async void` except for event handlers.
- Dispose owned `IDisposable` and `IAsyncDisposable` resources correctly.
- Prefer dependency injection and configuration abstractions over hard-coded
  environment values.
- Use structured logging with message templates. Do not log secrets or
  sensitive personal data.
- Choose collections and LINQ for readability, but avoid repeated enumeration,
  hidden expensive operations, and unnecessary allocations in hot paths.
- Make immutable state the default when practical.
- Keep public APIs small and intentional.

## ASP.NET Core Practices

- Use dependency injection rather than manually constructing service
  dependencies.
- Keep controllers and endpoint handlers thin; place business logic in focused
  services or domain types.
- Use correct HTTP methods, status codes, validation, and cancellation.
- Return consistent error responses without exposing implementation details.
- Keep configuration and secrets outside source code.
- Use asynchronous APIs for network, database, and file I/O.

## Testing and Verification

- Add or update focused tests for changed behavior.
- Test observable outcomes, important edge cases, validation, and failure paths.
- Keep tests deterministic, isolated, readable, and independent of execution
  order.
- Avoid mocking value objects or internal implementation details.
- Run the smallest relevant formatter, analyzer, build, and test commands.
- Resolve warnings introduced by the change and preserve existing behavior not
  covered by the request.

## Completion Checklist

Before finishing:

1. Confirm the implementation fully addresses the request.
2. Remove unnecessary complexity and duplication.
3. Check names, control flow, null handling, disposal, async behavior, and
   cancellation.
4. Ensure comments add useful context and remain accurate.
5. Verify formatting, analyzers, compilation, and relevant tests.
