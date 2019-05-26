using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwindSystem.Models;

[assembly: HostingStartup(typeof(NorthwindSystem.Areas.Identity.IdentityHostingStartup))]
namespace NorthwindSystem.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<NorthwindIdentityContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("NorthwindIdentityContextConnection")));

                services.AddDefaultIdentity<IdentityUser>()
                    .AddRoles<IdentityRole>()
                    .AddDefaultUI(UIFramework.Bootstrap4)
                    .AddEntityFrameworkStores<NorthwindIdentityContext>();
            });
        }
    }
}