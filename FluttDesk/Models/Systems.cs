using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluttDesk.Models
{
    [Table("TB_Systems")]
    public class Systems
    {
        [Key]
        public int Systemid { get; set; }
        public String SystemName { get; set; }
        public Byte[]? Systemlogo { get; set; }
    }
}
