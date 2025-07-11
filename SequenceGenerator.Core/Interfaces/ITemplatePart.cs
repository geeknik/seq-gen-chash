namespace SequenceGenerator.Core.Interfaces;

public interface ITemplatePart
{
    bool IsToken { get; }
}

public class LiteralPart : ITemplatePart
{
    public string Value { get; init; } = string.Empty;
    public bool IsToken => false;
}

public class TokenPart : ITemplatePart
{
    public string TokenName { get; init; } = string.Empty;
    public string? Format { get; init; }
    public Dictionary<string, string> Parameters { get; init; } = new();
    public bool IsToken => true;
}