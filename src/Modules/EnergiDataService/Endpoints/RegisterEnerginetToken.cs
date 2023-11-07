using Modules.Energinet.Domain;

namespace sgeltid.Modules.Energinet.Application;

public record RegisterEnerginetTokenCommand();

public class RegisterEnerginetTokenHandler
{
    //private readonly BackendDbContext context;

    //public RegisterEnerginetTokenHandler(BackendDbContext context) => this.context = context;

    public static async void Handle(RegisterEnerginetTokenCommand command)
    {
        EnerginetTokenId id = new EnerginetTokenId(command.Id);

        //bool exists = await context.EnerginetTokens.AnyAsync(t => t.Id == new TokenId(id));

        //if (exists)
        //{
        //    throw new ArgumentException("Energinet token already exists");
        //}

        EnerginetToken token = new EnerginetToken.CreateToken(id);

        //await context.EnerginetTokens.AddAsync(token);
        //await context.SaveChangesAsync();½
    }
}
