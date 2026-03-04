using HackerNewsGetterAPI.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HackerNewsGetterAPI.Extensions;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    { 
        ServiceDescriptor[] endpointServiceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsClass: true, IsAbstract: false, IsInterface: false } && 
                           type.IsAssignableTo(typeof(IApiEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IApiEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(endpointServiceDescriptors);

        return services;
    }

    public static IApplicationBuilder MapEndpoints(
        this WebApplication app)
    {
        IEnumerable<IApiEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IApiEndpoint>>();

        foreach (IApiEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }

}
