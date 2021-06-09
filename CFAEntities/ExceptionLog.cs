using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CFAEntities
{
    public class ExceptionLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
        public string MethodName { get; set; }
        public string ClassName { get; set; }
    }
}
