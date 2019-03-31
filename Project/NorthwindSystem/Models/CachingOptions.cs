using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindSystem.Models
{
    public class CachingOptions
    {
        public string DirectoryPath { get; set; }
        public int MaxImagesCount { get; set; }
        public TimeSpan CacheExpirationTime { get; set; }
    }
}
