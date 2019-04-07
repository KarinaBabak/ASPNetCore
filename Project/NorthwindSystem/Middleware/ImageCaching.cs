using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NorthwindSystem.Helpers;
using NorthwindSystem.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindSystem.Middleware
{
    public class ImageCaching
    {
        private const string imageType = "image";//"image/bmp";
        private readonly RequestDelegate _next;
        private CachingOptions _options;
        private IImageCacheHelper _cacheHelper;

        public ImageCaching(RequestDelegate next, IImageCacheHelper cacheHelper, CachingOptions options)
        {
            _next = next;
            _options = options;
            _cacheHelper = cacheHelper;
            _cacheHelper.InitCacheOptions(_options);
        }

        public async Task Invoke(HttpContext httpContext)
        {
            bool isImageContentType = httpContext.Response.ContentType?.Contains(imageType) ?? false;
            bool isImageType = httpContext.Request?.Path.Value.Contains(imageType, StringComparison.InvariantCultureIgnoreCase) ?? false;
            var responseStream = httpContext.Response.Body;
            if (!isImageType)
            {
                await _next(httpContext);
                return;
            }

            var imageId = httpContext.Request.Path.Value.Split('/').Last();
            var imageStream = await _cacheHelper.Get(imageId);

            if (imageStream != null)
            {
                await imageStream.CopyToAsync(responseStream);
                return;
            }

            using (var stream = new MemoryStream())
            {
                httpContext.Response.Body = stream;

                await _next(httpContext);
                await _cacheHelper.Save(stream, imageId);
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(responseStream);
            }
        }
}
}
