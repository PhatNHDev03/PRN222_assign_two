using FUNewsManagementSystem.DataAccess;
using FUNewsManagementSystem.DataAccess.IRepository;
using FUNewsManagementSystem.Services.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITagRepository,TagRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
            services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<INewsArticleService, NewsArticleService>();
            services.AddScoped<ISystemAccountService, SystemAccountService>();
            services.AddScoped<ITagService, TagService>();
            return services;
        }
        public static IServiceCollection AddDatabaseAndConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FUNewsManagementContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddSingleton<IConfiguration>(configuration);
            return services;
        }
    }
}
