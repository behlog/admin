using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;

namespace Microsoft.Extensions.DependencyInjection;


public static class ServiceProvider
{

    public static IMvcBuilder AddBehlogAdminArea(IMvcBuilder builder)
    {
        var assembly = typeof(HomeController).Assembly;
        
        builder.AddApplicationPart(assembly)
            .AddRazorRuntimeCompilation(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(assembly));
            });
        
        
        return builder;
    }


    public static IEndpointRouteBuilder AddBehlogAdminRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapAreaControllerRoute(
            name: "admin",
            areaName: "",
            pattern: "{area}/{controller=Home}/{action=Index}/{id?}");

        return endpoints;
    }
}