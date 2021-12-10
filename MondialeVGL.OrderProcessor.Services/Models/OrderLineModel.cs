using System;
using System.Xml.Serialization;

namespace MondialeVGL.OrderProcessor.Services.Models
{
    [Serializable]
    [XmlType(TypeName = "PurchaseOrderLine")]
    public class OrderLineModel
    {
        [XmlElement(ElementName = "LineNumber")]
        public int LineNumber { get; set; }

        [XmlElement(ElementName = "ProductDescription")]
        public string ProductDescription { get; set; }

        [XmlElement(ElementName = "OrderQty")]
        public int OrderQty { get; set; }
    }
}
