using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace NorthwindSystem.Middleware
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNodeModules(this IApplicationBuilder app, string rootPath)
        {
            var fileProvider = new PhysicalFileProvider(
                    Path.Combine(rootPath, "node_modules")
                );
            var options = new StaticFileOptions();
            options.RequestPath = "/node_modules";
            options.FileProvider = fileProvider;

            app.UseStaticFiles(options);
            return app;
        }
    }
}
