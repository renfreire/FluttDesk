using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluttDesk.Models
{
    [Table("TB_Users")]
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPwd { get; set; }
        public DateTime UserDtCreation { get; set; }
        public DateTime? UserDtLastLogin { get; set; }
        public string UserDepto { get; set; }
        public bool UserStatus { get; set; }
        public string UserRole { get; set; }
    }
}
