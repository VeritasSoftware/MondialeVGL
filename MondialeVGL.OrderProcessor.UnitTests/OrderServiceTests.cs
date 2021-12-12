using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MondialeVGL.OrderProcessor.Repository;
using MondialeVGL.OrderProcessor.Services;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Xunit;

namespace MondialeVGL.OrderProcessor.UnitTests
{
    public class OrderServiceTests
    {
        private readonly ServiceProvider _serviceProvider;

        public OrderServiceTests()
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

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task GetOrdersXmlAsync_Success()
        {
            //Arrange
            var repository = _serviceProvider.GetRequiredService<IOrderService>();

            //Act
            var ordersResult = await repository.GetOrdersXmlAsync();

            XDocument doc = null;

            var xmlBytes = Encoding.UTF8.GetBytes(ordersResult.OrdersXml);

            using (var memStream = new MemoryStream(xmlBytes))
            using (var xmlReader = XmlReader.Create(memStream))
            {
                doc = XDocument.Load(xmlReader);
            }

            //Assert
            Assert.NotNull(doc);
            Assert.Equal(4, doc.Descendants().Count(node => node.Name == XName.Get("PurchaseOrder")));
            Assert.NotNull(doc.Descendants().SingleOrDefault(node => node.Value == "PO2008-01"));
            Assert.NotNull(doc.Descendants().SingleOrDefault(node => node.Value == "PO2008-02"));
            Assert.NotNull(doc.Descendants().SingleOrDefault(node => node.Value == "PO2008-03"));
            Assert.NotNull(doc.Descendants().SingleOrDefault(node => node.Value == "PO2008-04"));
        }
    }
}
