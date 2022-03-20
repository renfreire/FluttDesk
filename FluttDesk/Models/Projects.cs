using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluttDesk.Models
{
    [Table("TB_Projects")]
    public class Projects
    {
        [Key]
        public int Projectid { get; set; }
        public String PrjTitle { get; set; }
        public String PrjEscope { get; set; }
        public int PrjRequester { get; set; }
        public String PrjDeptoRequester { get; set; }
        public int PrjSystem { get; set; }
        public String Deptoti { get; set; }
        public int PrjTechnicalResponsableid { get; set; }
        public int PrjTeamid { get; set; }
        public DateTime PrjDtCreation { get; set; }
        public DateTime PrjDtClosed { get; set; }
        public DateTime PrjLastChange { get; set; }
        public int ProjEstimatedHours { get; set; }
        public int PrjStatus { get; set; }
        public String PrjPPR { get; set; }
    }
}
