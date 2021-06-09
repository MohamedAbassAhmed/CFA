using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CFAEntities
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public int AgentNo { get; set; }
        public string PurchaseOrderStatus { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        //[ForeignKey("PurchaseOrderStatusNo")]
        //public PurchaseOrderStatus PurchaseOrderStatus { get; set; }
    }
}
