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
                    var orderService = serviceProvider.GetRequiredService<IOrderService>();

                    //Process orders
                    var ordersResult = await orderService.GetOrdersXmlAsync();

                    //Print Errors
                    foreach(var error in ordersResult.Errors)
                    {
                        Console.WriteLine($"Error: {error.InnerException?.Message ?? error.Message}\n");
                    }

                    //Print Orders Xml
                    Console.WriteLine(ordersResult.OrdersXml);
                }
                catch (Exception ex)
                {
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
    }
}
