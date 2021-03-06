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
        private readonly IServiceProvider _serviceProvider;

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
            //Arrange
            var repository = _serviceProvider.GetRequiredService<IOrderRepository>();

            //Act
            var ordersResult = await repository.GetOrdersAsync();

            //Assert
            Assert.Equal(4, ordersResult.Orders.Orders.Count);
            Assert.False(ordersResult.HasErrors);
            Assert.Equal("PO2008-01", ordersResult.Orders.Orders.ElementAt(0).Header.PurchaseOrderNumber);
            Assert.Equal(2, ordersResult.Orders.Orders.ElementAt(0).Details.Count);
            Assert.Equal("PO2008-02", ordersResult.Orders.Orders.ElementAt(1).Header.PurchaseOrderNumber);
            Assert.Equal(2, ordersResult.Orders.Orders.ElementAt(1).Details.Count);
            Assert.Equal("PO2008-03", ordersResult.Orders.Orders.ElementAt(2).Header.PurchaseOrderNumber);
            Assert.Equal(1, ordersResult.Orders.Orders.ElementAt(2).Details.Count);
            Assert.Equal("PO2008-04", ordersResult.Orders.Orders.ElementAt(3).Header.PurchaseOrderNumber);
            Assert.Equal(3, ordersResult.Orders.Orders.ElementAt(3).Details.Count);
        }

        [Fact]
        public async Task GetOrdersAsync_BadInputData_Failure()
        {
            //Arrange
            IOrderRepository repository = new OrderRepository(@".\Interface Data - Bad Data.csv");

            //Act
            var ordersResult = await repository.GetOrdersAsync();

            //Assert
            Assert.Equal(2, ordersResult.Orders.Orders.Count);
            Assert.True(ordersResult.HasErrors);
            Assert.Equal(3, ordersResult.Errors.Count);
            Assert.Equal("String '*/05/14' was not recognized as a valid DateTime.", ordersResult.Errors.ElementAt(0).InnerException.Message);
            Assert.Equal("*H is an invalid record type. Valid values [H|D].", ordersResult.Errors.ElementAt(1).Message);
            Assert.Contains(@"D,PO2008-04,1,GREEN BEDS,*&,", ordersResult.Errors.ElementAt(2).Message);
        }

        [Fact]
        public async Task GetOrdersAsync_OnReadError_BadInputData_Failure()
        {
            //Arrange
            var errors = new List<Exception>();
            
            IOrderRepository repository = new OrderRepository(@".\Interface Data - Bad Data.csv");            

            repository.OnReadError += async error =>
            {
                errors.Add(error);

                await Task.CompletedTask;
            };

            //Act
            var ordersResult = await repository.GetOrdersAsync();

            //Assert
            Assert.Equal(2, ordersResult.Orders.Orders.Count);
            Assert.True(ordersResult.HasErrors);
            Assert.Equal(3, ordersResult.Errors.Count);
            Assert.Equal(3, errors.Count);
            Assert.Equal("String '*/05/14' was not recognized as a valid DateTime.", errors.ElementAt(0).InnerException.Message);
            Assert.Equal("*H is an invalid record type. Valid values [H|D].", errors.ElementAt(1).Message);
            Assert.Contains(@"D,PO2008-04,1,GREEN BEDS,*&,", errors.ElementAt(2).Message);
        }
    }
}
