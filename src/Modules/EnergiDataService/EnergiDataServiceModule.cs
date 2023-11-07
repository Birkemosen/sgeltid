using sgeltid.Modules.Energinet.Application;
using Wolverine;

namespace Modules.Energinet;

public class EnergiDataServiceModule : IModule
{

    private const string baseUri = "/energidata";

    public void RegisterModule(IServiceCollection services)
    {
        // Register the dbContext
        services.AddDbContext<EnergiDataDbContext>(options =>
        {
            options.UseNpgsql(Configuration.GetConnectionString("EnergiDataDbConnection"));
        });

        // Register the EnergiData kWhPrices background service as a hosted service
        services.AddHostedService<kWhPriceBackgroundService>();

        return services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    public void AddRoutes(IEndpointRouteBuilder builder)
    {
        var endpoints = builder.MapGroup("/api")
            .WithOpenApi()
            .WithName("Energinet")
            .WithTags("Energinet");

        endpoints.MapPost("/energinet/tokens", (RegisterEnerginetTokenCommand createCommand, IMessageBus bus) => bus.InvokeAsync(createCommand));

        endpoints.MapPost("/energinet/prices", (GetEnerginetPricesCommand getCommand, IMessageBus bus) => bus.InvokeAsync(getCommand));

        /*endpoints.MapGet("/energinet/tokens/{id}", async (int id) =>
        {
            var token = await mediator.Send(new ReadEnerginetTokenQuery { Id = id });
            // Handle the response
        });

        endpoints.MapPut("/energinet/tokens/{id}", async (UpdateEnerginetTokenCommand updateCommand) =>
        {
            var token = await mediator.Send(updateCommand);
            // Handle the response
        });

        endpoints.MapDelete("/energinet/tokens/{id}", async (DeleteEnerginetTokenCommand deleteCommand) =>
        {
            var result = await mediator.Send(deleteCommand);
            // Handle the response
        });*/

    }
}