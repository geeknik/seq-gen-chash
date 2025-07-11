using SequenceGenerator.Core.Interfaces;

namespace SequenceGenerator.Core.Tokens;

public class DateTimeToken : IToken
{
    public string Name => "datetime";
    public bool RequiresStorage => false;
    public string? DefaultFormat { get; init; }

    public Task<string> GenerateAsync(ITokenContext context, CancellationToken cancellationToken = default)
    {
        var dateTime = context.GenerationTime;
        
        var format = DefaultFormat ?? "yyyyMMddHHmmss";
        if (context.Parameters.TryGetValue("format", out var formatParam) && formatParam is string formatStr && !string.IsNullOrEmpty(formatStr))
        {
            format = formatStr;
        }

        var result = dateTime.ToString(format);
        return Task.FromResult(result);
    }
}