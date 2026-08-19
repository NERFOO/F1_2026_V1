using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F1.Models
{
    [Table("V_RESULTRACE")]
    public class VistaResultRace
    {
        [Key]
        [Column("POSITION_RACE")]
        public int PositionRace { get; set; }

        [Column("LAP_TIME")]
        public string LapTime { get; set; }

        [Column("DRIVER_NAME")]
        public string DriverName { get; set; }

        [Column("TEAM_NAME")]
        public string TeamName { get; set; }

        [Column("POINTS")]
        public int Points { get; set; }

        [Column("ID_RACE")]
        public int IdRace { get; set; }
    }
}
