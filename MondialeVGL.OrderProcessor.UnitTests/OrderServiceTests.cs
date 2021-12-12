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
using System.Xml.XPath;
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
            var service = _serviceProvider.GetRequiredService<IOrderService>();

            //Act
            var ordersResult = await service.GetOrdersXmlAsync();

            XDocument doc = null;

            var xmlBytes = Encoding.UTF8.GetBytes(ordersResult.OrdersXml);

            using (var memStream = new MemoryStream(xmlBytes))
            using (var xmlReader = XmlReader.Create(memStream))
            {
                doc = XDocument.Load(xmlReader);
            }

            //Assert
            Assert.NotNull(doc);
            Assert.Equal(4, doc.Descendants(XName.Get("PurchaseOrder")).Count());
            Assert.Equal(4, doc.XPathSelectElements("//PurchaseOrders/PurchaseOrder").Count());
            Assert.NotNull(doc.Descendants().SingleOrDefault(node => node.Value == "PO2008-01"));
            Assert.Equal(2, doc.Descendants().SingleOrDefault(node => node.Value == "PO2008-01")
                                             .Parent
                                             .Descendants(XName.Get("Lines"))
                                             .Descendants(XName.Get("PurchaseOrderLine")).Count());
            Assert.Equal(2, doc.XPathSelectElements("//PurchaseOrders/PurchaseOrder[1]/Lines/PurchaseOrderLine").Count());
            Assert.NotNull(doc.Descendants().SingleOrDefault(node => node.Value == "PO2008-02"));
            Assert.Equal(2, doc.Descendants().SingleOrDefault(node => node.Value == "PO2008-02")
                                             .Parent
                                             .Descendants(XName.Get("Lines"))
                                             .Descendants(XName.Get("PurchaseOrderLine")).Count());
            Assert.Equal(2, doc.XPathSelectElements("//PurchaseOrders/PurchaseOrder[2]/Lines/PurchaseOrderLine").Count());
            Assert.NotNull(doc.Descendants().SingleOrDefault(node => node.Value == "PO2008-03"));
            Assert.Single(doc.Descendants().SingleOrDefault(node => node.Value == "PO2008-03")
                                             .Parent
                                             .Descendants(XName.Get("Lines"))
                                             .Descendants(XName.Get("PurchaseOrderLine")));
            Assert.Single(doc.XPathSelectElements("//PurchaseOrders/PurchaseOrder[3]/Lines/PurchaseOrderLine"));
            Assert.NotNull(doc.Descendants().SingleOrDefault(node => node.Value == "PO2008-04"));
            Assert.Equal(3, doc.Descendants().SingleOrDefault(node => node.Value == "PO2008-04")
                                             .Parent
                                             .Descendants(XName.Get("Lines"))
                                             .Descendants(XName.Get("PurchaseOrderLine")).Count());
            Assert.Equal(3, doc.XPathSelectElements("//PurchaseOrders/PurchaseOrder[4]/Lines/PurchaseOrderLine").Count());
        }
    }
}
