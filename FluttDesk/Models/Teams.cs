﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluttDesk.Models
{
    [Table("TB_Teams")]
    public class Teams
    {
        [Key]     
        public int TeamId { get; set; }        
        public String TeamName { get; set; }        
        public String TeamStatus { get; set; }
        //public ICollection<Members> Members { get; set; }
    }
}
