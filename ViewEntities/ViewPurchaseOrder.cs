using System;
using System.Collections.Generic;
using System.Text;

namespace ViewEntities
{
    public class ViewPurchaseOrder
    {
        public int Id { get; set; }
        public int AgentNo { get; set; }
        public string AgentName { get; set; }
        public string PurchaseOrderStatus { get; set; }
        public DateTime DateTime { get; set; }
    }
}
