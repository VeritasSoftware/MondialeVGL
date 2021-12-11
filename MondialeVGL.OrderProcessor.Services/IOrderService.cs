using MondialeVGL.OrderProcessor.Services.Models;
using System;
using System.Threading.Tasks;

namespace MondialeVGL.OrderProcessor.Services
{
    public interface IOrderService
    {
        Task<OrdersResult> GetOrdersXmlAsync();
        event Func<Exception, Task> OnReadError;
    }
}