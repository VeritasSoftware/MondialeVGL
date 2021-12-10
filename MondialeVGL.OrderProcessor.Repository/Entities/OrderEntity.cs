using System.Collections.Generic;

namespace MondialeVGL.OrderProcessor.Repository.Entities
{
    public class OrderEntity
    {
        public OrderHeaderEntity Header { get; set; }

        public ICollection<OrderDetailEntity> Details { get; set; }
    }
}
