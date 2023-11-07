using sgeltid.Infrastructure;
using sgeltid.Modules.Energinet.Domain;

namespace Modules.Energinet.Domain;

public record EnerginetTokenId(Guid id) : GuidEntityId(id);

public class EnerginetToken
{
    public enum Status { Created = 0, Ready = 10, Expired = 20 }

    public int Id { get; set; }
    public string Token { get; private set; } = null!;
    public DateTime Expires { get; private set; }

    public Status TokenStatus { get; private set; }
    /// <summary>
    /// For EntityFramework
    /// </summary>
    /// <param name="Id"></param>
    private EnerginetToken(EnerginetTokenId Id) : base(Id) { }

    /// <summary>
    /// Creates a new Energinet Token with the minimum fields allowed
    /// </summary>
    /// <param name="tokenId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static EnerginetToken CreateToken(EnerginetTokenId tokenId, string token)
    {
        EnerginetToken energinetToken = new EnerginetToken(tokenId) { Token = token, TokenStatus = Status.Created, Expires = DateTime.Today.AddYears(1) };
        //token.AddDomainEvent(new EnerginetTokenCreated(tokenId));

        return energinetToken;
    }
}