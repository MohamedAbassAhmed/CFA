using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CFAEntities
{
    public class LoadDeliveryStock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int LoadDeliveryNo { get; set; }
        [ForeignKey("LoadDeliveryNo")]
        public LoadDelivery LoadDelivery { get; set; }
        public int Quantity { get; set; }
    }
}
