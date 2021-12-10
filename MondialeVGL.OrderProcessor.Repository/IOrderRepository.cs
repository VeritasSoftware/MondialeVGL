using MondialeVGL.OrderProcessor.Repository.Entities;
using System.Collections.Generic;

namespace MondialeVGL.OrderProcessor.Repository
{
    public interface IOrderRepository
    {
        IAsyncEnumerable<OrderEntity> GetOrdersAsync();
    }
}