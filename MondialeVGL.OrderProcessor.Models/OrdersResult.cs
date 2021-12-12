using System;
using System.Collections.Generic;
using System.Linq;

namespace MondialeVGL.OrderProcessor.Models
{
    public class OrdersResult
    {
        public string OrdersXml { get; set; }
        public bool HasErrors => Errors.Any();
        public IEnumerable<Exception> Errors { get; set; } = new List<Exception>();
    }
}
