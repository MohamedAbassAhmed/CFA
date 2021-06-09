using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CFAEntities
{
    public class LoadDelivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FishTypeNo { get; set; }
        [ForeignKey("FishTypeNo")]
        public FishType FishType { get; set; }
        public DateTime LoadDeliveryDate { get; set; } = DateTime.Now;
        public int FishBoatNo { get; set; }
        [ForeignKey("FishBoatNo")]
        public FishBoat FishBoat { get; set; }
        public string BatchNo { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
