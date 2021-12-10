using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MondialeVGL.OrderProcessor.Repository;
using System;
using System.Threading.Tasks;

namespace MondialeVGL.OrderProcessor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //Read appsettings
            var builder = new ConfigurationBuilder()
                            .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();

            //Build DI container
            var services = new ServiceCollection();

            services.AddScoped<IOrderRepository>(sp => new OrderRepository(config["OrdersFilePath"]));

            var serviceProvider = services.BuildServiceProvider();

            var repository = serviceProvider.GetRequiredService<IOrderRepository>();

            await foreach (var order in repository.GetOrdersAsync())
            {
                Console.WriteLine($"PO No: {order.Header.PurchaseOrderNumber} No of Details: {order.Details.Count}");
            }

            Console.ReadLine();
        }
    }
}
