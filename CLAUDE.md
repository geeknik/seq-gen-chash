# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a C# sequence generator project designed to create customizable sequences for invoices, IDs, and other formatted identifiers. The project is currently in the design phase with no implementation yet.

## Development Environment Setup

1. **Install .NET 8 SDK**:
   ```bash
   brew install --cask dotnet-sdk
   ```

2. **Initialize the project** (when starting implementation):
   ```bash
   dotnet new sln -n SequenceGenerator
   dotnet new classlib -n SequenceGenerator.Core
   dotnet new xunit -n SequenceGenerator.Tests
   dotnet new console -n SequenceGenerator.CLI
   dotnet sln add SequenceGenerator.Core/SequenceGenerator.Core.csproj
   dotnet sln add SequenceGenerator.Tests/SequenceGenerator.Tests.csproj
   dotnet sln add SequenceGenerator.CLI/SequenceGenerator.CLI.csproj
   ```

## Common Commands

Since the project hasn't been implemented yet, here are the standard .NET commands that will be used:

- **Build**: `dotnet build`
- **Run tests**: `dotnet test`
- **Run a specific test**: `dotnet test --filter "FullyQualifiedName~TestClassName.TestMethodName"`
- **Run CLI**: `dotnet run --project SequenceGenerator.CLI`
- **Add package**: `dotnet add package <PackageName>`
- **Restore packages**: `dotnet restore`

## Architecture Overview

The project follows a clean architecture pattern with these key components:

1. **Core Interfaces**:
   - `ISequenceDefinition` - Defines user templates
   - `ISequenceStorage` - Persistence abstraction
   - `IToken` - Token interface for format string elements

2. **Main Components**:
   - `SequenceTemplateParser` - Parses format strings like "INV-{yyyyMM}-{seq:000}"
   - `SequenceGenerator` - Generates sequences using parsed templates
   - Storage implementations (InMemory, potentially SQLite)

3. **Token Types** (planned):
   - Sequential counters with formatting
   - Date/time tokens
   - Random values (UUID, alphanumeric)
   - Potentially: checksum tokens, custom plugins

## Important Design Decisions

1. **Format String Syntax**: Uses curly braces `{token:format}` similar to .NET string interpolation
2. **Storage Abstraction**: Pluggable storage backends via `ISequenceStorage`
3. **Thread Safety**: Generators must be thread-safe for concurrent ID generation
4. **Extensibility**: Token system designed for future plugin support
5. **Library Consideration**: SmartFormat.NET suggested for template engine functionality

## Testing Strategy

- Unit tests for all public interfaces
- Integration tests for storage implementations
- Concurrency tests for thread-safe sequence generation
- Format string parsing edge cases

## Security Considerations

- No hardcoded secrets or API keys
- Validate all user-provided format strings
- Sanitize storage keys to prevent injection
- Consider rate limiting for sequence generation in production

## References

- **DESIGN.md**: Contains detailed architecture diagrams and implementation phases
- **CODING.md**: General coding standards and principles
- Consider SmartFormat.NET (https://github.com/axuno/SmartFormat) for template functionality