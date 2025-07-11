namespace SequenceGenerator.Core.Interfaces;

public interface ISequenceTemplateParser
{
    ParsedTemplate Parse(string template);
}

public class ParsedTemplate
{
    public List<ITemplatePart> Parts { get; init; } = new();
}