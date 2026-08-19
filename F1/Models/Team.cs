using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F1.Models
{
	[Table("TEAM")]
	public class Team
	{
		[Key]
		[Column("ID_TEAM")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int IdTeam { get; set; }

		[Required]
		[Column("TEAM_NAME")]
		public string TeamName { get; set; }

		[Required]
		[Column("PRICE")]
		public decimal Price { get; set; }

		[Required]
		[Column("TOTAL_POINTS")]
		public int TotalPoints { get; set; }

		[Required]
		[Column("TEAM_IMG")]
		public string TeamImg { get; set; }
	}
}
