using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MondialeVGL.OrderProcessor.Repository;
using MondialeVGL.OrderProcessor.Services;
using System;

namespace MondialeVGL.OrderProcessor
{
    public static class Extensions
    {
        public static IServiceProvider AddOrderProcessor(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOrderRepository>(sp => new OrderRepository(configuration["OrdersFilePath"]));
            services.AddScoped<IOrderService, OrderService>();
            services.AddSingleton<IMappingService, MappingService>();

            return services.BuildServiceProvider();
        }
    }
}
