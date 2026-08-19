using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F1.Models
{
	[Table("V_LEAGUE")]
	public class VistaLeague
	{
		[Key]
        [Column("POSICION")]
        public int Posicion { get; set; }

        [Column("ID_LEAGUE")]
		public int IdLeague { get; set; }

		[Column("LEAGUE_NAME")]
		public string LeagueName { get; set; }

		[Column("NICKNAME")]
		public string Nickname { get; set; }

		[Column("USER_TOTAL_POINTS")]
		public int UserTotalPoints { get; set; }

        [Column("ID_USER")]
        public int IdUSer { get; set; }
    }
}
