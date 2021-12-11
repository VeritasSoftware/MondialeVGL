using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MondialeVGL.OrderProcessor.Repository;
using System;
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

            var orders = await repository.GetOrdersAsync();

            Assert.Equal("PO No: PO2008-01, No of Details: 2", $"PO No: {orders.Orders.ElementAt(0).Header.PurchaseOrderNumber}, No of Details: {orders.Orders.ElementAt(0).Details.Count}");
            Assert.Equal("PO No: PO2008-02, No of Details: 2", $"PO No: {orders.Orders.ElementAt(1).Header.PurchaseOrderNumber}, No of Details: {orders.Orders.ElementAt(1).Details.Count}");
            Assert.Equal("PO No: PO2008-03, No of Details: 1", $"PO No: {orders.Orders.ElementAt(2).Header.PurchaseOrderNumber}, No of Details: {orders.Orders.ElementAt(2).Details.Count}"); ;
            Assert.Equal("PO No: PO2008-04, No of Details: 3", $"PO No: {orders.Orders.ElementAt(3).Header.PurchaseOrderNumber}, No of Details: {orders.Orders.ElementAt(3).Details.Count}");
        }

        [Fact]
        public async Task GetOrdersAsync_Failure()
        {
            var exceptions = new List<Exception>();

            OrderRepository.OnReadError += async exception =>
            {
                lock(this)
                {
                    exceptions.Add(exception);
                }               

                await Task.CompletedTask;
            };

            var repository = new OrderRepository(@".\Interface Data - Bad Data.csv");

            var orders = await repository.GetOrdersAsync();

            Assert.Equal(2, orders.Orders.Count);
            Assert.Equal(3, exceptions.Count);
            Assert.Equal("String '*/05/14' was not recognized as a valid DateTime.", exceptions.ElementAt(0).InnerException.Message);
            Assert.Equal("*H is an invalid record type. Valid values [H|D].", exceptions.ElementAt(1).Message);
            Assert.Contains(@"D,PO2008-04,1,GREEN BEDS,*&,", exceptions.ElementAt(2).Message);
        }
    }
}
