using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F1.Models
{
    [Table("V_USER_CLASSIFFICATION")]
    public class VistaUserClassification
    {
        [Key]
        [Column("POSICION")]
        public int Posicion { get; set; }

        [Column("ID_USER")]
        public int IdUser { get; set; }

        [Column("USER_TOTAL_POINTS")]
        public int UserTotalPoints { get; set; }

        [Column("ID_LEAGUE")]
        public int IdLeague { get; set; }
    }
}
