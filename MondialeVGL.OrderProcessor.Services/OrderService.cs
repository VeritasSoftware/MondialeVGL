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

        public event Func<Exception, Task> OnReadError;

        public OrderService(IOrderRepository orderRepository, IMappingService mappingService)
        {
            _orderRepository = orderRepository;
            _mappingService = mappingService;

            _orderRepository.OnReadError += OrderRepository_OnReadError;
        }

        private async Task OrderRepository_OnReadError(Exception arg)
        {
            OnReadError?.Invoke(arg);

            await Task.CompletedTask;
        }

        public async Task<OrdersResult> GetOrdersXmlAsync()
        {
            var ordersEntityResult = await _orderRepository.GetOrdersAsync();

            var orderModels = _mappingService.Map<OrderEntityCollection, OrderModelCollection>(ordersEntityResult.Orders);

            var ordersXml = orderModels.Serialize();

            var ordersResult = new OrdersResult
            {
                OrdersXml = ordersXml,
                Errors = ordersEntityResult.Errors
            };

            return ordersResult;
        }

        public async ValueTask DisposeAsync()
        {
            _orderRepository.OnReadError -= OrderRepository_OnReadError;

            await Task.CompletedTask;
        }
    }
}
