using MondialeVGL.OrderProcessor.Repository;
using MondialeVGL.OrderProcessor.Repository.Entities;
using MondialeVGL.OrderProcessor.Services.Models;
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
            var orderEntities = await _orderRepository.GetOrdersAsync();

            var orderModels = _mappingService.Map<OrderCollectionEntity, OrderCollectionModel>(orderEntities);

            return orderModels.Serialize();
        }
    }
}
