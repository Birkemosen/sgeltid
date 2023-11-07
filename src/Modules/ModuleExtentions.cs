public namespace sgeltid.Modules;

public class ModuleExtensions
{
    // this could alsp be added into the DI container
    static readonly List<IModule> registeredModules = new List<IModule>();

    public static IServiceCollection RegisterModules(this IServiceCollection services)
    {
        var modules = DiscoverModules();
        foreach (var module in modules)
        {
            module.RegisterModule(services);
            registeredModules.Add(module);
        }

        return services;
    }

    public static WebApplicationBuilder AddEndpoints(this WebApplicationBuilder app)
    {
        var modules = app.Services.GetServices<IModule>();
        foreach (var module in modules)
        {
            module.AddEndpoints(app);
        }

        return app;
    }

    private static IEnumerable<IModule> DiscoverModules()
    {
        return typeof(IModule).Assembly
            .GetTypes()
            .Where(p => p.Class && p.IsAssignableTo(typeof(IModule)))
            .Select(Activator.CreateInstance)
            .Cast<IModule>();
    }
}