using CsvHelper.Configuration.Attributes;
using System;

namespace MondialeVGL.OrderProcessor.Repository.Entities
{
    public class OrderHeaderEntity : OrderBaseEntity
    {        
        [Index(2)]
        public string Supplier { get; set; }

        [Index(3)]
        public string Origin { get; set; }

        [Index(4)]
        public string Destination { get; set; }

        [Index(5)]
        [Format("d/MM/yy")]
        public DateTime CargoReadyDate { get; set; }
    }
}
