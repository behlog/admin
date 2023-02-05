using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.FileProviders;
using Behlog.Web.Admin.ViewModelProviders;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceProvider
{
    /// <summary>
    /// Adds Behlog Admin Area as an ASP.NET CORE MVC module.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IMvcBuilder AddBehlogAdminArea(this IMvcBuilder builder)
    {
        var assembly = typeof(HomeController).Assembly;
        
        builder.AddApplicationPart(assembly)
            .AddRazorRuntimeCompilation(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(assembly));
            });
        
        
        return builder;
    }

    /// <summary>
    /// Adds Behlog Admin services.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddBehlogAdminServices(this IServiceCollection services)
    {
        services.AddScoped<IAdminContentViewModelProvider, AdminContentViewModelProvider>();

        return services;
    }
    
    /// <summary>
    /// Adds Behlog Admin routing.
    /// </summary>
    /// <param name="endpoints"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder AddBehlogAdminRoutes(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapAreaControllerRoute(
            name: "admin",
            areaName: WebsiteAreaNames.Admin,
            pattern: "{area}/{controller=Home}/{action=Index}/{id?}");

        return endpoints;
    }
}