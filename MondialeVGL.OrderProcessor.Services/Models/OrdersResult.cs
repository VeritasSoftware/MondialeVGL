using System;
using System.Collections.Generic;

namespace MondialeVGL.OrderProcessor.Services.Models
{
    public class OrdersResult
    {
        public string OrdersXml { get; set; }
        public IEnumerable<Exception> Errors { get; set; }
    }
}
