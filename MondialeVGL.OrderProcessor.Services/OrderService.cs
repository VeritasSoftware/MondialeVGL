using MondialeVGL.OrderProcessor.Repository;
using MondialeVGL.OrderProcessor.Repository.Entities;
using MondialeVGL.OrderProcessor.Services.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MondialeVGL.OrderProcessor.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMappingService _mappingService;

        public OrderService(IOrderRepository orderRepository, IMappingService mappingService)
        {
            _orderRepository = orderRepository;
            _mappingService = mappingService;
        }

        public async Task<string> GetOrdersXmlAsync()
        {
            var orders = new OrderCollectionModel
            {
                Orders = new List<OrderModel>()
            };

            await foreach (var order in _orderRepository.GetOrdersAsync())
            {
                var orderModel = _mappingService.Map<OrderEntity, OrderModel>(order);

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
