# Sequence Generator

A customizable sequence generator for creating formatted identifiers like invoice numbers, document IDs, and other sequential values.

## Features

- **Flexible Template System**: Use tokens like `{seq}`, `{date}`, `{year}`, etc.
- **Sequential Counters**: Auto-incrementing numbers with custom formatting
- **Date/Time Tokens**: Include timestamps in various formats
- **Thread-Safe**: Concurrent generation with proper synchronization
- **Pluggable Storage**: Currently includes in-memory storage with interface for custom implementations
- **Batch Generation**: Generate multiple sequences at once

## Quick Start

### Build and Run

```bash
# Install .NET 9 SDK (macOS)
brew install --cask dotnet-sdk

# Build the solution
dotnet build

# Run tests
dotnet test

# Run the CLI
dotnet run --project SequenceGenerator.CLI
```

### Example Templates

- Invoice Number: `INV-{yyyyMM}-{seq:000}`
- Order ID: `ORDER-{year}-{seq:00000}`
- Document ID: `DOC-{date:yyyyMMdd}-{seq:0000}`
- Simple Counter: `{seq:000000}`

### Available Tokens

- `{seq:format}` - Sequential counter with custom format
- `{date:format}` - Current date/time with .NET format string
- `{year}` - Current year (4 digits)
- `{month}` - Current month (2 digits)
- `{day}` - Current day (2 digits)
- `{yyyyMM}` - Year and month
- `{yyyyMMdd}` - Year, month, and day

## Architecture

```
SequenceGenerator.Core/
├── Interfaces/           # Core abstractions
├── Models/              # Data models
├── Parsing/             # Template parser
├── Storage/             # Storage implementations
├── Tokens/              # Token implementations
└── SequenceGenerator.cs # Main generator

SequenceGenerator.Tests/  # Unit tests
SequenceGenerator.CLI/    # Command-line interface
```

## Usage Example

```csharp
// Create dependencies
var storage = new InMemoryStorage();
var parser = new SequenceTemplateParser();
var tokenFactory = new TokenFactory();
var generator = new SequenceGenerator(parser, storage, tokenFactory);

// Define a sequence
var definition = new SequenceDefinition
{
    Id = "invoice",
    Name = "Invoice Number",
    Template = "INV-{yyyyMM}-{seq:000}"
};

// Generate sequences
var invoice1 = await generator.GenerateAsync(definition); // INV-202507-001
var invoice2 = await generator.GenerateAsync(definition); // INV-202507-002
```

## Extending

### Custom Tokens

Implement the `IToken` interface:

```csharp
public class RandomToken : IToken
{
    public string Name => "random";
    public bool RequiresStorage => false;
    
    public Task<string> GenerateAsync(ITokenContext context, CancellationToken cancellationToken = default)
    {
        // Implementation
    }
}
```

Register with the token factory:

```csharp
tokenFactory.RegisterToken("random", () => new RandomToken());
```

### Custom Storage

Implement the `ISequenceStorage` interface for persistent storage (e.g., SQLite, Redis).

## License

This project is provided as-is for educational and commercial use.