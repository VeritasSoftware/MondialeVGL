using CsvHelper.Configuration.Attributes;

namespace MondialeVGL.OrderProcessor.Repository.Entities
{
    public abstract class OrderBaseEntity
    {
        [Index(0)]
        public virtual string RecordType { get; set; }

        [Index(1)]
        public string PurchaseOrderNumber { get; set; }
    }
}
