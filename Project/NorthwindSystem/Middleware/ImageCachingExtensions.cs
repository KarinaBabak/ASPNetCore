using Microsoft.AspNetCore.Builder;
using NorthwindSystem.Models;

namespace NorthwindSystem.Middleware
{
    public static class ImageCachingExtensions
    {
        public static IApplicationBuilder UseImageCaching(this IApplicationBuilder app, CachingOptions options)
        {
            return app.UseMiddleware<ImageCaching>(options);
        }
    }
}
