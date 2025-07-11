namespace SequenceGenerator.Core.Interfaces;

public interface ITokenContext
{
    ISequenceDefinition Definition { get; }
    ISequenceStorage Storage { get; }
    Dictionary<string, object> Parameters { get; }
    DateTime GenerationTime { get; }
}