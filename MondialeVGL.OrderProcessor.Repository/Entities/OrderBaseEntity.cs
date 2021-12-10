using CsvHelper.Configuration.Attributes;

namespace MondialeVGL.OrderProcessor.Repository.Entities
{
    public abstract class OrderBaseEntity
    {
        public virtual RecordType Type { get; }

        [Index(1)]
        public string PurchaseOrderNumber { get; set; }
    }
}
