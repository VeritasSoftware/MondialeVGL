using MondialeVGL.OrderProcessor.Repository;
using MondialeVGL.OrderProcessor.Services.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MondialeVGL.OrderProcessor.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<string> GetOrdersXmlAsync()
        {
            var orders = new OrderCollectionModel
            {
                Orders = new List<OrderModel>()
            };

            await foreach (var order in _orderRepository.GetOrdersAsync())
            {
                //I would do this mapping between the Entity & Model
                //using a mapping library eg. Automapper
                var orderModel = new OrderModel
                {
                    PurchaseOrderNumber = order.Header.PurchaseOrderNumber,
                    Supplier = order.Header.Supplier,
                    Destination = string.IsNullOrEmpty(order.Header.Destination)? GetDestination(order.Header.Supplier) : order.Header.Destination,
                    Origin = order.Header.Origin,
                    CargoReady = order.Header.CargoReadyDate.ToString("yyyy-MM-dd"),
                    Lines = order.Details.Select(x => new OrderLineModel
                    {
                        LineNumber = x.LineNumber,
                        ProductDescription = x.ItemDescription,
                        OrderQty = x.OrderQty
                    }).ToList()
                };

                orders.Orders.Add(orderModel);
            }

            return orders.Serialize();
        }

        private static string GetDestination(string supplier)
        {
            return supplier switch
            {
                "SHANGHAI FURNITURE COMPANY" => "Melbourne AUMEL",
                "YANTIAN INDUSTRIAL PRODUCTS" => "Sydney AUSYD",
                _ => null
            };
        }
    }
}
