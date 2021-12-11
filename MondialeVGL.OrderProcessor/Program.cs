using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MondialeVGL.OrderProcessor.Repository;
using MondialeVGL.OrderProcessor.Services;
using System;
using System.Threading.Tasks;

namespace MondialeVGL.OrderProcessor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Read appsettings
            var builder = new ConfigurationBuilder()
                            .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();

            //Build DI container
            var services = new ServiceCollection();

            services.AddScoped<IOrderRepository>(sp => new OrderRepository(config["OrdersFilePath"]));
            services.AddScoped<IOrderService, OrderService>();
            services.AddSingleton<IMappingService, MappingService>();

            var serviceProvider = services.BuildServiceProvider();

            try
            {
                OrderService.OnReadingExceptionOccurred += OrderService_OnReadingExceptionOccurred;

                var orderService = serviceProvider.GetRequiredService<IOrderService>();

                //Process orders
                var ordersXml = await orderService.GetOrdersXmlAsync();

                Console.WriteLine(ordersXml);

                await serviceProvider.DisposeAsync();
            }
            catch(Exception ex)
            {
                await serviceProvider.DisposeAsync();

                OrderService.OnReadingExceptionOccurred -= OrderService_OnReadingExceptionOccurred;

                Console.WriteLine(ex.Message);
            }            

            Console.ReadLine();
        }

        private static async Task OrderService_OnReadingExceptionOccurred(Exception arg)
        {
            Console.WriteLine($"Error: {arg.InnerException?.Message}\n");

            await Task.CompletedTask;
        }
    }
}
