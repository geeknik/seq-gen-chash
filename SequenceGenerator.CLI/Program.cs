using SequenceGenerator.Core;
using SequenceGenerator.Core.Models;
using SequenceGenerator.Core.Parsing;
using SequenceGenerator.Core.Storage;
using SequenceGenerator.Core.Tokens;

var storage = new InMemoryStorage();
var parser = new SequenceTemplateParser();
var tokenFactory = new TokenFactory();
var generator = new SequenceGenerator.Core.SequenceGenerator(parser, storage, tokenFactory);

Console.WriteLine("=== Sequence Generator CLI ===");
Console.WriteLine();

while (true)
{
    Console.WriteLine("Choose an option:");
    Console.WriteLine("1. Generate a sequence");
    Console.WriteLine("2. Generate batch sequences");
    Console.WriteLine("3. View examples");
    Console.WriteLine("4. Exit");
    Console.Write("\nOption: ");
    
    var option = Console.ReadLine();
    
    switch (option)
    {
        case "1":
            await GenerateSingle(generator);
            break;
        case "2":
            await GenerateBatch(generator);
            break;
        case "3":
            ShowExamples();
            break;
        case "4":
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.\n");
            break;
    }
}

async Task GenerateSingle(SequenceGenerator.Core.SequenceGenerator gen)
{
    Console.Write("\nEnter sequence name: ");
    var name = Console.ReadLine() ?? "Unnamed";
    
    Console.Write("Enter template (e.g., INV-{yyyyMM}-{seq:000}): ");
    var template = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(template))
    {
        Console.WriteLine("Template cannot be empty.\n");
        return;
    }
    
    try
    {
        var definition = new SequenceDefinition
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            Template = template
        };
        
        var result = await gen.GenerateAsync(definition);
        Console.WriteLine($"\nGenerated: {result}\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}\n");
    }
}

async Task GenerateBatch(SequenceGenerator.Core.SequenceGenerator gen)
{
    Console.Write("\nEnter sequence name: ");
    var name = Console.ReadLine() ?? "Unnamed";
    
    Console.Write("Enter template (e.g., INV-{yyyyMM}-{seq:000}): ");
    var template = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(template))
    {
        Console.WriteLine("Template cannot be empty.\n");
        return;
    }
    
    Console.Write("Enter number of sequences to generate: ");
    if (!int.TryParse(Console.ReadLine(), out var count) || count <= 0)
    {
        Console.WriteLine("Invalid count.\n");
        return;
    }
    
    try
    {
        var definition = new SequenceDefinition
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            Template = template
        };
        
        var results = await gen.GenerateBatchAsync(definition, count);
        Console.WriteLine("\nGenerated sequences:");
        foreach (var result in results)
        {
            Console.WriteLine($"  {result}");
        }
        Console.WriteLine();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}\n");
    }
}

void ShowExamples()
{
    Console.WriteLine("\n=== Template Examples ===");
    Console.WriteLine("Invoice Number:     INV-{yyyyMM}-{seq:000}");
    Console.WriteLine("Order ID:           ORDER-{year}-{seq:00000}");
    Console.WriteLine("Document ID:        DOC-{date:yyyyMMdd}-{seq:0000}");
    Console.WriteLine("Sequential Only:    {seq:000000}");
    Console.WriteLine("Date Only:          {date:yyyy-MM-dd}");
    Console.WriteLine("Complex:            {year}/{month}/BATCH-{seq:000}-{day}");
    Console.WriteLine("\nTokens available:");
    Console.WriteLine("  {seq:format}      - Sequential counter");
    Console.WriteLine("  {date:format}     - Date/time with custom format");
    Console.WriteLine("  {year}            - Current year (4 digits)");
    Console.WriteLine("  {month}           - Current month (2 digits)");
    Console.WriteLine("  {day}             - Current day (2 digits)");
    Console.WriteLine("  {yyyyMM}          - Year and month");
    Console.WriteLine("  {yyyyMMdd}        - Year, month, and day");
    Console.WriteLine();
}