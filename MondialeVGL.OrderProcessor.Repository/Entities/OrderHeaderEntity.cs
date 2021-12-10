using CsvHelper.Configuration.Attributes;
using System;

namespace MondialeVGL.OrderProcessor.Repository.Entities
{
    public class OrderHeaderEntity : OrderBaseEntity
    {
        public override RecordType Type => RecordType.H;
        
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
