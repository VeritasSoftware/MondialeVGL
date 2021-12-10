using System.Collections.Generic;

namespace MondialeVGL.OrderProcessor.Repository.Entities
{
    public class OrderCollectionEntity
    {
        public ICollection<OrderEntity> Orders { get; set; }
    }
}
