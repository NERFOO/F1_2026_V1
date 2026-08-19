using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F1.Models
{
	[Table("USER_TEAM")]
	public class UserTeam
	{
		[Key]
		[Column("ID_USER_TEAM")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int IdUserTeam { get; set; }

		[Required]
		[Column("USER_TEAM_NAME")]
		public string UserTeamName { get; set; }

		[Required]
		[Column("TEAM_MONEY")]
		public decimal TeamMoney { get; set; }

		[Required]
		[Column("ID_USER")]
		public int IdUser { get; set; }

		[Required]
		[Column("ID_TEAM")]
		public int IdTeam { get; set; }

		// ✅ Navegaciones correctas
		public UserPlayer User { get; set; }
		public Team Team { get; set; }
		public ICollection<DriverUserTeam> DriverUserTeams { get; set; }
	}
}
