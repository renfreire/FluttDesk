using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluttDesk.Models
{
    [Table("TB_Status")]
    public class Status_activities
    {
        [Key]
        public int Statusid { get; set; }
        public String StatusName { get; set; }
        public String StatusColor { get; set; }
    }
}
