using Microsoft.AspNetCore.Builder;

namespace NorthwindSystem.Middleware
{
    public static class ImageCachingExtensions
    {
        public static IApplicationBuilder UseImageCaching(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageCaching>();
        }
    }
}
