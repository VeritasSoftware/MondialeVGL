using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MondialeVGL.OrderProcessor.Services.Models
{
    [Serializable]
    [XmlType(TypeName = "PurchaseOrders")]
    public class OrderModelCollection
    {
        [XmlArray("PurchaseOrder")]
        [XmlArrayItem("PurchaseOrder")]
        public List<OrderModel> Orders { get; set; }
    }
}
