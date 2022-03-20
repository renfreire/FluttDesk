using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluttDesk.Models
{
    [Table("TB_Roles")]
    public class Roles
    {
        [Key]
        public int Roleid { get; set; }
        public String RoleName { get; set; }
        public String RoleStatus { get; set; }
    }
}
