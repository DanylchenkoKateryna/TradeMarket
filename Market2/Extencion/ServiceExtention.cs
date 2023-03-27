using Business.Interfaces;
using Business.Services;
using Data.Data;
using Data.Entities;
using Data.FluentValidation;
using Data.Interfaces;
using FluentValidation;
using LoggerService;
using Market2.ErrorModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TradeMarketProject.LogerService;

namespace TradeMarketProject.Extencion
{
    public static class ServiceExtention
    {
        public static void ConfigureCors(this IServiceCollection services) =>
         services.AddCors(options =>
         {
             options.AddPolicy("CorsPolicy",
                 builder => {
                     builder.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
                 });
         });
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
        services.Configure<IISOptions>(options =>
        {


        });
        public static void ConfigureLoggerService(this IServiceCollection services) =>
               services.AddScoped<ILoggerManager, LoggerManager>();

        public static void ConfigureSqlContext(this IServiceCollection services,
        IConfiguration configuration) =>
         services.AddDbContext<TradeMarketDbContext>(opts =>
         opts.UseSqlServer(configuration.GetConnectionString("Market")));

        public static void ConfigureUnitOfWork(this IServiceCollection services) =>
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IReceiptService, ReceiptService>();
            services.AddScoped<IStatisticService, StatisticService>();
            services.AddScoped<IErrorMessageProvider, ErrorMessageProvider>();
        }

        public static void ConfigureFluentValidation(this IServiceCollection services)
        {
            services.AddScoped<IValidator<Customer>, CustomerValidation>();
            services.AddScoped<IValidator<ProductCategory>, ProductCategoryValidation>();
            services.AddScoped<IValidator<Product>, ProductValidation>();
            services.AddScoped<IValidator<ReceiptDetail>, ReceiptDetailValidation>();
            services.AddScoped<IValidator<Receipt>, ReceiptValidation>();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TradeMarket API", Version = "v1" });
            });
        }
    }
}
