using System;
using System.Collections.Generic;
using System.Text;

namespace ViewEntities
{
    public class ViewFishTypeStock
    {
        public int FishTypeNo { get; set; }
        public string FishTypeName { get; set; }
        public int Quantity { get; set; }
        public int LoadDeliveryNo { get; set; }
        public int LoadDeliverStockNo { get; set; }
    }
}
