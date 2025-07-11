using System.Text.RegularExpressions;
using SequenceGenerator.Core.Interfaces;

namespace SequenceGenerator.Core.Parsing;

public partial class SequenceTemplateParser : ISequenceTemplateParser
{
    [GeneratedRegex(@"\{([^}:]+)(?::([^}]+))?\}", RegexOptions.Compiled)]
    private static partial Regex TokenRegex();

    public ParsedTemplate Parse(string template)
    {
        if (string.IsNullOrEmpty(template))
        {
            throw new ArgumentException("Template cannot be null or empty", nameof(template));
        }

        var parts = new List<ITemplatePart>();
        var lastIndex = 0;
        var matches = TokenRegex().Matches(template);

        foreach (Match match in matches)
        {
            if (match.Index > lastIndex)
            {
                parts.Add(new LiteralPart { Value = template[lastIndex..match.Index] });
            }

            var tokenName = match.Groups[1].Value;
            var format = match.Groups[2].Success ? match.Groups[2].Value : null;

            var parameters = new Dictionary<string, string>();
            
            if (!string.IsNullOrEmpty(format) && format.Contains(','))
            {
                var formatParts = format.Split(',');
                format = formatParts[0];
                
                for (int i = 1; i < formatParts.Length; i++)
                {
                    var paramParts = formatParts[i].Split('=');
                    if (paramParts.Length == 2)
                    {
                        parameters[paramParts[0].Trim()] = paramParts[1].Trim();
                    }
                }
            }

            parts.Add(new TokenPart 
            { 
                TokenName = tokenName,
                Format = format,
                Parameters = parameters
            });

            lastIndex = match.Index + match.Length;
        }

        if (lastIndex < template.Length)
        {
            parts.Add(new LiteralPart { Value = template[lastIndex..] });
        }

        return new ParsedTemplate { Parts = parts };
    }
}