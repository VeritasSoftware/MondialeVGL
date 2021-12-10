using System.Threading.Tasks;

namespace MondialeVGL.OrderProcessor.Services
{
    public interface IOrderService
    {
        Task<string> GetOrdersXmlAsync();
    }
}