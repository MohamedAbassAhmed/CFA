using System;
using System.Collections.Generic;
using System.Text;

namespace ViewEntities
{
    public class ViewLoadDelivery
    {
        public int Id { get; set; }
        public int FishTypeNo { get; set; }
        public string FishTypeName{ get; set; }
       
        public DateTime LoadDeliveryDate { get; set; }
        public int FishBoatNo { get; set; }
        public string FishBoatName { get; set; }
       
        public string BatchNo { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; }
    }
}
