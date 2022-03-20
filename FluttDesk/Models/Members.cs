using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluttDesk.Models
{
    [Table("TB_Members")]
    public class Members
    {
        [Key]
        public int Memberid { get; set; }        
        [ForeignKey("Teams")]
        public int Teamid { get; set; }
        public virtual Teams Teams { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
        public virtual Users Users { get; set; }
        public int MemberRole { get; set; }
    }
}
