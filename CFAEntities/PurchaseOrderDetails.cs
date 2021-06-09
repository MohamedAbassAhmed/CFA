using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CFAEntities
{
    public class PurchaseOrderDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PurchaseOrderNo { get; set; }
        [ForeignKey("PurchaseOrderNo")]
        public PurchaseOrder PurchaseOrder { get; set; }
        public int FishTypeNo { get; set; }
        [ForeignKey("FishTypeNo")]
        public FishType FishType { get; set; }
        public int Quantity { get; set; }
    }
}
