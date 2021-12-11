using MondialeVGL.OrderProcessor.Repository.Entities;
using System;
using System.Threading.Tasks;

namespace MondialeVGL.OrderProcessor.Repository
{
    public interface IOrderRepository
    {
        Task<OrdersEntityResult> GetOrdersAsync();
        event Func<Exception, Task> OnReadError;
    }
}