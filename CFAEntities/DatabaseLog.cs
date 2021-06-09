using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CFAEntities
{
    public class DatabaseLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }
        public string UserName { get; set; }
        public string PageName { get; set; }
        public string Operation { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
    }
}
