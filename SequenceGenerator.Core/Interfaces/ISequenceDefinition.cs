namespace SequenceGenerator.Core.Interfaces;

public interface ISequenceDefinition
{
    string Id { get; }
    string Name { get; }
    string Template { get; }
    string? Description { get; }
    DateTime CreatedAt { get; }
    DateTime? LastUsedAt { get; }
}