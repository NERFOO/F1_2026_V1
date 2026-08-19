using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F1.Models
{
    [Table("LEAGUE")]
    public class League
    {
        [Key]
        [Column("ID_LEAGUE")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int IdLeague { get; set; }

        [Required]
        [Column("LEAGUE_NAME")]
        public string LeagueName { get; set; }

        [Required]
        [Column("LEAGUE_COD")]
        public int LeagueCode { get; set; }
    }
}
