using System;
using System.Collections.Generic;
using System.Linq;

namespace MondialeVGL.OrderProcessor.Repository.Entities
{
    public class OrdersEntityResult
    {
        public OrderEntityCollection Orders { get; set; }
        public bool HasErrors => Errors.Any();
        public ICollection<Exception> Errors { get; set; }

        public OrdersEntityResult()
        {
            this.Orders = new OrderEntityCollection
            {
                Orders = new List<OrderEntity>()
            };

            this.Errors = new List<Exception>();
        }
    }
}
