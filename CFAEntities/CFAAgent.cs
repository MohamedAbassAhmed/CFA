using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CFAEntities
{
    public class CFAAgent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [RegularExpression("^[A-Za-z ]+$", ErrorMessage = "The name must contain only letters and spaces")]
        public string AgentName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
