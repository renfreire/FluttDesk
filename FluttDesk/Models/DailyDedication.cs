using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluttDesk.Models
{
    [Table("TB_DailyDedication")]
    public class DailyDedication
    {
        [Key]
        public int Dailyid { get; set; }
        public String DailyName { get; set; }
        public String DailyHours { get; set; }
    }
}
