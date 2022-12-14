using Data.Data;
using Data.Interfaces;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeMarketProject.LogerService;

namespace TradeMarketProject.Extencion
{
    public static class ServiceExtention
    {
        public static void ConfigureCors(this IServiceCollection services) =>
         services.AddCors(options =>
         {
             options.AddPolicy("CorsPolicy", builder =>
             builder.AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());
         });
         public static void ConfigureIISIntegration(this IServiceCollection services) =>
         services.Configure<IISOptions>(options =>
         {

             
         });
        //Register to DI Container
        public static void ConfigureLoggerService(this IServiceCollection services) =>
               services.AddScoped<ILoggerManager, LoggerManager>();

        public static void ConfigureSqlContext(this IServiceCollection services,
        IConfiguration configuration) =>
         services.AddDbContext<TradeMarketDbContext>(opts =>
         opts.UseSqlServer(configuration.GetConnectionString("Market")));

        public static void ConfigureUnitOfWork(this IServiceCollection services) =>
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
