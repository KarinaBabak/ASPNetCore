using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NorthwindSystem.Filters
{
    public class ActionLoggingFilterFactory : IFilterFactory
    {
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new ActionCallsLogger(serviceProvider.GetService<ILoggerFactory>(), serviceProvider.GetService<IConfiguration>());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
