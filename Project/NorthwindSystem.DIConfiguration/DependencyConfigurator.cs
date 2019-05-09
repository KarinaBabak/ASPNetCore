using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NorthwindSystem.BLL;
using NorthwindSystem.BLL.Implementation;
using NorthwindSystem.BLL.Interface;
using NorthwindSystem.Data;
using NorthwindSystem.Persistence.Implementation;
using NorthwindSystem.Persistence.Interface;

namespace NorthwindSystem.DIConfiguration
{
    public static class DependencyConfigurator
    {
        public static void RegisterBLLDependencies(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<ISupplierService, SupplierService>();
            services.AddTransient<ILocalConfiguration, BLLConfiguration>();
        }

        public static void RegisterPersistenceDependencies(this IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
        }

        public static void RegisterInfrastractureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NorthwindSystemContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("NorthwindSystemDbConnection")));
        }

        public static void RegisterAutoMapperDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
