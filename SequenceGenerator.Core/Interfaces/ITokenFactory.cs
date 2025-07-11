namespace SequenceGenerator.Core.Interfaces;

public interface ITokenFactory
{
    IToken? CreateToken(string tokenName);
    void RegisterToken(string name, Func<IToken> factory);
}