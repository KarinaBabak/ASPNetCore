using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NorthwindSystem.Helpers;
using NorthwindSystem.Models;
using System;
using System.Collections.Generic;
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

            if (!isImageType)
            {
                await _next(httpContext);
                return;
            }

            var imageId = httpContext.Request.Path.Value.Split('/').Last();
            var stream = await _cacheHelper.GetOrCreate(imageId);
            await Proceed(httpContext, stream);
        }

        private async Task Proceed(HttpContext httpContext, Stream stream)
        {
            //using (var stream = new MemoryStream())
            {
                Stream responseBody = httpContext.Response.Body;
                httpContext.Response.Body = stream;
                //stream.Seek(0, SeekOrigin.Begin);
                await _next(httpContext);
            }
        }
}
}
