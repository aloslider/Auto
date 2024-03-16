using Auto.WebAPI.Features;
using Microsoft.Extensions.DependencyInjection.Extensions;

static class MinimalEndpointExtensions
{
    internal static IApplicationBuilder RegisterMinimalEndpoints(this WebApplication app, string routePrefix = "")
    {
        var rootGroup = app.MapGroup(routePrefix); 
        var endpoints = app.Services.GetRequiredService<IEnumerable<IMinimalEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapRoutes(rootGroup);
        }

        return app;
    }

    internal static IServiceCollection AddMinimalEndpoints(this IServiceCollection services)
    {
        var assembly = typeof(Program).Assembly;
        var serviceDescriptors = 
            assembly.DefinedTypes
                .Where(type => !type.IsAbstract && 
                               !type.IsInterface &&
                               type.IsAssignableTo(typeof(IMinimalEndpoint)))
                .Select(type => ServiceDescriptor.Transient(typeof(IMinimalEndpoint), type));
        services.TryAddEnumerable(serviceDescriptors);
        return services;
    }
}