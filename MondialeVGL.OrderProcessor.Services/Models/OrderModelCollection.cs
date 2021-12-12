using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MondialeVGL.OrderProcessor.Services.Models
{   
    [Serializable]
    [XmlRoot("PurchaseOrders")]
    public class OrderModelCollection
    {
        [XmlElement("PurchaseOrder")]
        public List<OrderModel> Orders { get; set; }
    }
}
