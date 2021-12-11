using System.Collections.Generic;

namespace MondialeVGL.OrderProcessor.Repository.Entities
{
    public class OrderEntityCollection
    {
        public ICollection<OrderEntity> Orders { get; set; }
    }
}
