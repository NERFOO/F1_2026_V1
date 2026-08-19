using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F1.Models
{
    [Table("V_USERTEAM")]
    public class VistaUserTeam
    {
        [Key]
        [Column("POSICION")]
        public int Posicion { get; set; }

        [Column("ID_USER")]
        public int IdUSer { get; set; }

        [Column("ID_USER_TEAM")]
        public int IdUserTeam { get; set; }

        [Column("USER_TEAM_NAME")]
        public string UserTeamName { get; set; }

        [Column("BUDGET")]
        public Decimal Budget { get; set; }

        [Column("DRIVER_NAME")]
        public string DriverName { get; set; }

        [Column("DRIVER_PRICE")]
        public Decimal DriverPrice { get; set; }

        [Column("DRIVER_IMG")]
        public string DriverImg { get; set; }

        [Column("TEAM")]
        public string TeamName { get; set; }

        [Column("TEAM_PRICE")]
        public Decimal TeamPrice { get; set; }

        [Column("TEAM_IMG")]
        public string TeamImg { get; set; }
    }
}
