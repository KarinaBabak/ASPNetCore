using NorthwindSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindSystem.Helpers
{
    public interface IImageCacheHelper
    {
        void InitCacheOptions(CachingOptions cacheOptions);
        Task<Stream> Get(string key);
        Task Save(Stream imageStream, string key);
    }
}
