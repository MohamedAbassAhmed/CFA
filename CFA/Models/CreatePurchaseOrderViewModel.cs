using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CFA.Models
{
    public class CreatePurchaseOrderViewModel
    {
        public int FishTypeNo { get; set; }
        public string FishTypeName { get; set; }
        [Range(0,int.MaxValue)]
        public int Quantity { get; set; }
    }
}
