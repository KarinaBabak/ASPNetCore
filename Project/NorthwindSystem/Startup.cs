using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwindSystem.Middleware;
using Microsoft.Extensions.Logging;
using NorthwindSystem.Helpers;
using NorthwindSystem.Models;
using NorthwindSystem.Filters;
using NorthwindSystem.DIConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using NorthwindSystem.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace NorthwindSystem
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.RegisterAutoMapperDependencies(Configuration);

            services.RegisterInfrastractureDependencies(Configuration);

            services.RegisterPersistenceDependencies();
            services.RegisterBLLDependencies();

            services.AddSingleton<IImageCacheHelper, FileImageCacheHelper>();

            RegisterIdentityDependencies(services);

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddMvc(options =>
            {
                options.Filters.Add(new ActionLoggingFilterFactory());

            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime, ILogger<Startup> logger)
        {
            applicationLifetime.ApplicationStarted.Register(() =>
            {
                OnApplicationStart(env, logger);
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseNodeModules(env.ContentRootPath);
            app.UseCookiePolicy();

            app.UseImageCaching(new CachingOptions
            {
                DirectoryPath = "C:/NorthwindSystem/CachedImages",
                MaxImagesCount = 5,
                CacheExpirationTime = new TimeSpan(0, 0, 60)
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "images",
                   template: "images/{categoryId}",
                   defaults: new { controller = "Category", action = "GetImage" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");              
            });
        }

        private void OnApplicationStart(IHostingEnvironment hostingEnvironment, ILogger<Startup> logger)
        {
            logger.LogInformation($"Application {hostingEnvironment.ApplicationName} starting " +
                $"work at {hostingEnvironment.ContentRootPath}");
            logger.LogInformation("Application configuration reading start");

            foreach (var configEntry in Configuration.AsEnumerable())
            {
                logger.LogTrace($"{configEntry.Key} : {configEntry.Value}");
            }

            logger.LogInformation("Application configuration reading end");
        }

        private void RegisterIdentityDependencies(IServiceCollection services)
        {
            services.AddDbContext<NorthwindIdentityContext>(options =>
                    options.UseSqlServer(
                        Configuration.GetConnectionString("NorthwindIdentityContextConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                    .AddRoles<IdentityRole>()
                    .AddDefaultUI(UIFramework.Bootstrap4)
                    .AddEntityFrameworkStores<NorthwindIdentityContext>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            // Azure AD authentication
            //services.AddAuthentication()
            //    .AddOpenIdConnect(AzureADDefaults.AuthenticationScheme, "AzureAD", options =>
            //    {
            //        Configuration.Bind("AzureAd", options);
            //    });
        }
    }
}
