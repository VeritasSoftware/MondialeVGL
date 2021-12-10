using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MondialeVGL.OrderProcessor.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MondialeVGL.OrderProcessor.UnitTests
{
    public class OrderRepositoryTests
    {
        private readonly ServiceProvider _serviceProvider;

        public OrderRepositoryTests()
        {
             //Read appsettings
             var builder = new ConfigurationBuilder()
                             .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();

            //Build DI container
            var services = new ServiceCollection();

            services.AddScoped<IOrderRepository>(sp => new OrderRepository(config["OrdersFilePath"]));

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task GetOrdersAsync_Success()
        {
            var repository = _serviceProvider.GetRequiredService<IOrderRepository>();

            var pos = new List<string>();

            await foreach (var order in repository.GetOrdersAsync())
            {
                pos.Add($"PO No: {order.Header.PurchaseOrderNumber}, No of Details: {order.Details.Count}");
            }

            Assert.Equal("PO No: PO2008-01, No of Details: 2", pos.ElementAt(0));
            Assert.Equal("PO No: PO2008-02, No of Details: 2", pos.ElementAt(1));
            Assert.Equal("PO No: PO2008-03, No of Details: 1", pos.ElementAt(2));
            Assert.Equal("PO No: PO2008-04, No of Details: 3", pos.ElementAt(3));
        }
    }
}
