using SequenceGenerator.Core.Interfaces;

namespace SequenceGenerator.Core.Tokens;

public class SequentialToken : IToken
{
    public string Name => "seq";
    public bool RequiresStorage => true;

    public async Task<string> GenerateAsync(ITokenContext context, CancellationToken cancellationToken = default)
    {
        var sequenceId = $"{context.Definition.Id}_seq";
        var nextValue = await context.Storage.GetNextSequenceValueAsync(sequenceId, cancellationToken);
        
        var format = "D";
        if (context.Parameters.TryGetValue("format", out var formatParam) && formatParam is string formatStr && !string.IsNullOrEmpty(formatStr))
        {
            // Handle format like "000" or "0000" by checking if all chars are 0
            if (formatStr.All(c => c == '0'))
            {
                format = $"D{formatStr.Length}";
            }
            else if (int.TryParse(formatStr, out var paddingLength))
            {
                format = $"D{paddingLength}";
            }
            else
            {
                format = formatStr;
            }
        }

        return nextValue.ToString(format);
    }
}