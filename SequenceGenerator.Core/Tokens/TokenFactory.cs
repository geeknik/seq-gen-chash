using SequenceGenerator.Core.Interfaces;

namespace SequenceGenerator.Core.Tokens;

public class TokenFactory : ITokenFactory
{
    private readonly Dictionary<string, Func<IToken>> _tokenFactories = new(StringComparer.OrdinalIgnoreCase);

    public TokenFactory()
    {
        RegisterDefaultTokens();
    }

    public IToken? CreateToken(string tokenName)
    {
        return _tokenFactories.TryGetValue(tokenName, out var factory) ? factory() : null;
    }

    public void RegisterToken(string name, Func<IToken> factory)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Token name cannot be null or empty", nameof(name));

        if (factory == null)
            throw new ArgumentNullException(nameof(factory));

        _tokenFactories[name] = factory;
    }

    private void RegisterDefaultTokens()
    {
        RegisterToken("seq", () => new SequentialToken());
        RegisterToken("sequence", () => new SequentialToken());
        RegisterToken("date", () => new DateTimeToken());
        RegisterToken("datetime", () => new DateTimeToken());
        RegisterToken("time", () => new DateTimeToken());
        RegisterToken("year", () => new DateTimeToken { DefaultFormat = "yyyy" });
        RegisterToken("month", () => new DateTimeToken { DefaultFormat = "MM" });
        RegisterToken("day", () => new DateTimeToken { DefaultFormat = "dd" });
        RegisterToken("yyyyMM", () => new DateTimeToken { DefaultFormat = "yyyyMM" });
        RegisterToken("yyyyMMdd", () => new DateTimeToken { DefaultFormat = "yyyyMMdd" });
    }
}