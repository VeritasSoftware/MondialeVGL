using System;
using System.Collections.Generic;
using System.Linq;

namespace MondialeVGL.OrderProcessor.Entities
{
    public class OrdersEntityResult
    {
        public OrderEntityCollection Orders { get; set; } = new OrderEntityCollection
        {
            Orders = new List<OrderEntity>()
        };
        public bool HasErrors => Errors.Any();
        public ICollection<Exception> Errors { get; set; } = new List<Exception>();
    }
}
