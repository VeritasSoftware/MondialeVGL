using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MondialeVGL.OrderProcessor.Models
{
    [Serializable]
    [XmlType(TypeName = "PurchaseOrder")]
    public class OrderModel
    {
        [XmlElement(ElementName = "CustomerPo")]
        public string PurchaseOrderNumber { get; set; }

        [XmlElement(ElementName = "Supplier")]
        public string Supplier { get; set; }

        [XmlElement(ElementName = "Origin")]
        public string Origin { get; set; }

        [XmlElement(ElementName = "Destination")]
        public string Destination { get; set; }

        [XmlElement(ElementName = "CargoReady")]
        public string CargoReady { get; set; }

        [XmlArray("Lines")]
        [XmlArrayItem("PurchaseOrderLine")]
        public List<OrderLineModel> Lines { get; set; }
    }
}
