using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CFAEntities
{
    public class FishType
    {
        [Key]
        public int Id { get; set; }
        public string FishTypeName { get; set; }
    }
}
