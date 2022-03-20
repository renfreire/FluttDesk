using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluttDesk.Models
{
    [Table("TB_Activities")]
    public class ActivitiesProject
    {
        [Key]
        public int Activityid { get; set; }
        public int ActProjectid { get; set; }
        public DateTime ActDtCreation { get; set; }
        public String ActPhases { get; set; }
        public int ActOrder { get; set; }
        public String ActSubject { get; set; }
        public String ActEscope { get; set; }
        public String ActStatus { get; set; }
        public int ActResposableid { get; set; }
        public int ActDailyDedication { get; set; }
        public int ActAttachment { get; set; }

        public int ActPercentCompleted { get; set; }
        public int ActCreatedby { get; set; }
    }
}
