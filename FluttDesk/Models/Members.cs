using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluttDesk.Models
{
    [Table("TB_Members")]
    public class Members
    {
        [Key]
        public int Memberid { get; set; }
        
        [ForeignKey("Teamid")]
        public virtual Teams Teams { get; set; }
        public int Teamid { get; set; }
        public int Userid { get; set; }
        public int MemberRole { get; set; }
    }
}
