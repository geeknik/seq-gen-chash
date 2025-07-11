Project: Customizable Sequence Generator (Invoice/ID Generator)

⸻

🧠 Purpose

The goal is to develop or integrate a flexible, extensible sequence generation engine in C# that allows users to define their own formatting rules for IDs like invoice numbers. These sequences must support:
	•	Fixed elements (e.g., "INV-", "-US")
	•	Dynamic date/time tokens (e.g., {yyyyMM}, {yyyy}, {MM})
	•	Incrementing counters with optional scoping (e.g., reset every month or year)
	•	Expression-based logic, such as "{prefix}-{year}{month}-{seq:000}"

⸻

💡 Key Features
	•	User-defined format strings, using a mini-template language
	•	Dynamic token resolution (date/time, user-defined variables)
	•	Sequence state tracking, persisted in-memory or via pluggable storage
	•	Scoped counters, e.g., a new sequence per customer, per month, etc.
	•	NuGet-packagable for reuse across internal/external projects
	•	Thread-safe, ideally async-friendly

⸻

💻 Development Environment
	•	OS: MacOS (Ventura+ recommended)
	•	Language: C# 10+
	•	Framework: .NET 8 SDK
Install via Homebrew: brew install --cask dotnet-sdk
	•	Package Manager: NuGet
	•	Editor: Visual Studio for Mac, JetBrains Rider, or dotnet CLI + VS Code

⸻

🏗️ Architecture

+-------------------------+
|   ISequenceDefinition   |  ← Interface for user-defined templates
+-------------------------+
             |
             ↓
+-------------------------+
|   SequenceTemplateParser|  ← Parses format string into token tree
+-------------------------+
             |
             ↓
+-------------------------+
|    SequenceGenerator    |  ← Uses parsed template & state to generate values
+-------------------------+
             |
             ↓
+-------------------------+
|    ISequenceStorage     |  ← Interface for persistence layer
+-------------------------+
     ↑           ↑
+----------+ +----------------+
| InMemory | | SqliteStorage? | ← Swappable backends
+----------+ +----------------+


⸻

🧬 Format String Design

Use a lightweight format syntax:

"INV-{yyyyMM}-{seq:000}"
"PO-{customerCode}-{year}-{seq:0000}"
"2025-{month}-{seq:000}"

Token Support

Token	Description
{yyyy}	Full year (e.g., 2025)
{MM}	Month padded (01–12)
{dd}	Day padded (01–31)
{seq:N}	Sequence number, padded
{customer}	Custom scoped variables
{guid}	Optional unique identifier


⸻

🔐 Sequence Isolation / Scope

Sequences should support isolation based on context:

var next = generator.GetNext("INV-{yyyyMM}-{seq:000}", scope: "2025-07");

Where "2025-07" becomes the partition key for seq.

⸻

🧪 Example Usage

var template = "INV-{yyyyMM}-{seq:000}";
var generator = new SequenceGenerator(new InMemorySequenceStore());

var id1 = await generator.GetNextAsync(template); // "INV-202507-001"
var id2 = await generator.GetNextAsync(template); // "INV-202507-002"


⸻

🔍 Existing Libraries (to evaluate or fork)
	•	Humanizer – for string manipulation (may assist with dynamic tokens)
	•	Scriban – a lightweight C# template engine (can be used to render format strings)
	•	SmartFormat.NET – very close to what you want: template formatting engine with support for custom tokens and formatters

🧩 You may be able to use SmartFormat.NET with a custom source + formatter to implement {seq:000}-style tokens.

⸻

🧰 Implementation Plan

Phase 1: MVP
	•	Parser for format strings
	•	Built-in tokens: date parts, counter
	•	In-memory state tracking (dictionary of counters)
	•	Basic unit tests
	•	CLI demo

Phase 2: Pluggable Storage
	•	Interface for storage
	•	Implement SQLite backend via Dapper or EF Core
	•	Add reset strategies (monthly, yearly, manual)

Phase 3: Advanced Tokens
	•	User-defined tokens (e.g., {customer})
	•	Expression evaluation (basic math/logic)
	•	Custom token registration API

⸻

🚀 Publishing & Distribution
	•	Package as a NuGet package:

dotnet pack -c Release
dotnet nuget push bin/Release/*.nupkg --source <your-feed>


	•	Optional CLI Tool:

dotnet new tool-manifest
dotnet tool install --local SequenceCli



⸻

📎 Related Projects / Inspirations
	•	Microsoft Power Automate format strings
	•	Apache Velocity / Liquid Templates (for DSL inspiration)
	•	SmartFormat.NET source for inspiration or integration

⸻

🧠 Assumptions
	•	.NET Core used across platforms
	•	Your team prefers open, inspectable code over opaque SaaS libraries
	•	Flexibility and extensibility are more important than raw performance

⸻

📚 References
	•	.NET SDK for MacOS
	•	SmartFormat Custom Formatters
	•	Scriban Templates

⸻

❗Open Questions
	•	Should we support backward/forward-compatible format changes?
	•	Need for concurrency-safe counters in distributed scenarios?
	•	Allow user UI to build sequences visually or only via string?
