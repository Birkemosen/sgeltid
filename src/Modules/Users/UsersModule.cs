using Wolverine;
using Modules.Users.Application;

namespace Modules.Users;

public class UserModule : IModule
{
    private const string baseUri = "/user";

    public void AddRoutes(IEndpointRouteBuilder builder)
    {
        var endpoints = builder.MapGroup("/api")
            .WithOpenApi()
            .WithName("User")
            .WithTags("User");

        endpoints.MapGet(baseUri, () => { return Results.Ok("{\"users\":[]}"); })
            .WithName("Get users");

        endpoints.MapGet(baseUri + "/{userId}", (int id) => { return Results.Ok("{\"users\":[" + id.ToString() + "]}"); })
            .WithName("Get User Details");

        endpoints.MapPost(baseUri + "/create", (CreateUserCommand cmd, IMessageBus bus)
            => bus.InvokeAsync(cmd))
            .WithName("Create User");
    }
}