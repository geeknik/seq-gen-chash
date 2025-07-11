using System.Text;
using SequenceGenerator.Core.Interfaces;
using SequenceGenerator.Core.Models;

namespace SequenceGenerator.Core;

public class SequenceGenerator : ISequenceGenerator
{
    private readonly ISequenceTemplateParser _parser;
    private readonly ISequenceStorage _storage;
    private readonly ITokenFactory _tokenFactory;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public SequenceGenerator(
        ISequenceTemplateParser parser,
        ISequenceStorage storage,
        ITokenFactory tokenFactory)
    {
        _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
    }

    public async Task<string> GenerateAsync(ISequenceDefinition definition, CancellationToken cancellationToken = default)
    {
        if (definition == null)
            throw new ArgumentNullException(nameof(definition));

        var parsedTemplate = _parser.Parse(definition.Template);
        var context = new TokenContext
        {
            Definition = definition,
            Storage = _storage,
            GenerationTime = DateTime.UtcNow
        };

        var result = new StringBuilder();

        foreach (var part in parsedTemplate.Parts)
        {
            if (part is LiteralPart literalPart)
            {
                result.Append(literalPart.Value);
            }
            else if (part is TokenPart tokenPart)
            {
                var token = _tokenFactory.CreateToken(tokenPart.TokenName);
                if (token == null)
                {
                    throw new InvalidOperationException($"Unknown token: {tokenPart.TokenName}");
                }

                foreach (var param in tokenPart.Parameters)
                {
                    context.Parameters[param.Key] = param.Value;
                }

                if (!string.IsNullOrEmpty(tokenPart.Format))
                {
                    context.Parameters["format"] = tokenPart.Format;
                }

                string tokenValue;
                if (token.RequiresStorage)
                {
                    await _semaphore.WaitAsync(cancellationToken);
                    try
                    {
                        tokenValue = await token.GenerateAsync(context, cancellationToken);
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
                else
                {
                    tokenValue = await token.GenerateAsync(context, cancellationToken);
                }

                result.Append(tokenValue);
                context.Parameters.Clear();
            }
        }

        if (definition is SequenceDefinition mutableDef)
        {
            mutableDef.LastUsedAt = DateTime.UtcNow;
        }

        return result.ToString();
    }

    public async Task<IEnumerable<string>> GenerateBatchAsync(ISequenceDefinition definition, int count, CancellationToken cancellationToken = default)
    {
        if (count <= 0)
            throw new ArgumentException("Count must be greater than 0", nameof(count));

        var results = new List<string>();
        
        for (int i = 0; i < count; i++)
        {
            results.Add(await GenerateAsync(definition, cancellationToken));
        }

        return results;
    }
}