public namespace sgeltid.Modules;
public interface IModule
{
    IServiceCollection RegisterModule(IServiceCollection services);
    IEndpointRouteBuilder AddEndpoints(IEndpointRoutBuilder endpoints);
}