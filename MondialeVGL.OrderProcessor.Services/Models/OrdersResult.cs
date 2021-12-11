using System;
using System.Collections.Generic;
using System.Linq;

namespace MondialeVGL.OrderProcessor.Services.Models
{
    public class OrdersResult
    {
        public string OrdersXml { get; set; }
        public bool HasErrors => Errors.Any();
        public IEnumerable<Exception> Errors { get; set; }

        public OrdersResult()
        {
            Errors = new List<Exception>();
        }
    }
}
