using MondialeVGL.OrderProcessor.Repository.Entities;
using System.Threading.Tasks;

namespace MondialeVGL.OrderProcessor.Repository
{
    public interface IOrderRepository
    {
        Task<OrderCollectionEntity> GetOrdersAsync();
    }
}