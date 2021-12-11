using MondialeVGL.OrderProcessor.Repository;
using MondialeVGL.OrderProcessor.Repository.Entities;
using MondialeVGL.OrderProcessor.Services.Models;
using System;
using System.Threading.Tasks;

namespace MondialeVGL.OrderProcessor.Services
{
    public class OrderService : IOrderService, IAsyncDisposable
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMappingService _mappingService;

        public static event Func<Exception, Task> OnReadError;

        public OrderService(IOrderRepository orderRepository, IMappingService mappingService)
        {
            _orderRepository = orderRepository;
            _mappingService = mappingService;

            OrderRepository.OnReadError += OrderRepository_OnReadError;
        }

        private async Task OrderRepository_OnReadError(Exception arg)
        {
            OnReadError?.Invoke(arg);

            await Task.CompletedTask;
        }

        public async Task<string> GetOrdersXmlAsync()
        {
            var orderEntities = await _orderRepository.GetOrdersAsync();

            var orderModels = _mappingService.Map<OrderCollectionEntity, OrderCollectionModel>(orderEntities);

            return orderModels.Serialize();
        }

        public async ValueTask DisposeAsync()
        {
            OrderRepository.OnReadError -= OrderRepository_OnReadError;

            await Task.CompletedTask;
        }
    }
}
