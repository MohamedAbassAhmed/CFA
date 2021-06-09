using System;
using System.Collections.Generic;
using System.Text;

namespace ViewEntities
{
    public class ViewLoadDeliveryStock
    {
        public int Id { get; set; }
        public int LoadDeliveryNo { get; set; }
        public int Quantity { get; set; }
        public int FishTypeNo { get; set; }
        public DateTime LoadDeliveryDate { get; set; }
        public string FishTypeName { get; set; }
    }
}
