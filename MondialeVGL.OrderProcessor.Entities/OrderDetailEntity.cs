using CsvHelper.Configuration.Attributes;

namespace MondialeVGL.OrderProcessor.Entities
{
    public class OrderDetailEntity : OrderBaseEntity
    {        
        [Index(2)]
        public int LineNumber { get; set; }

        [Index(3)]
        public string ItemDescription { get; set; }

        [Index(4)]
        public int OrderQty { get; set; }
    }
}
