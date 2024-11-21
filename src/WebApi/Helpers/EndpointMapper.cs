using System.Reflection;

namespace WalmgateIdentity.WebApi.Helpers;

public static class EndpointMapper
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpointTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IEndpoint)) && !t.IsInterface);

        foreach (var type in endpointTypes)
        {
            var endpoint = Activator.CreateInstance(type) as IEndpoint;
            endpoint?.Map(app);
        }
    }
}
