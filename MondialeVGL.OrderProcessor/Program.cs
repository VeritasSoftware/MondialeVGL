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
            try
            {
                //Read appsettings
                var builder = new ConfigurationBuilder()
                                .AddJsonFile($"appsettings.json", true, true);

                var config = builder.Build();

                //Build DI container
                var services = new ServiceCollection();
                var serviceProvider = services.AddOrderProcessor(config);

                try
                {
                    OrderService.OnReadError += OrderService_OnReadError;

                    var orderService = serviceProvider.GetRequiredService<IOrderService>();

                    //Process orders
                    var ordersXml = await orderService.GetOrdersXmlAsync();

                    Console.WriteLine(ordersXml);
                }
                catch (Exception ex)
                {
                    OrderService.OnReadError -= OrderService_OnReadError;

                    Console.WriteLine(ex.Message);
                }

                Console.ReadLine();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);

                Console.ReadLine();
            }
        }

        private static async Task OrderService_OnReadError(Exception arg)
        {
            Console.WriteLine($"Error: {arg.InnerException?.Message??arg.Message}\n");

            await Task.CompletedTask;
        }
    }
}
