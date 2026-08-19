using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F1.Models
{
	[Table("DRIVER_USER_TEAM")]
	public class DriverUserTeam
	{
		[Key]
		[Column("ID_DRIVER_TEAM")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int IdDriverTeam { get; set; }

		[Required]
		[Column("ID_USER_TEAM")]
		public int IdUserTeam { get; set; }

		[Required]
		[Column("ID_DRIVER")]
		public int IdDriver { get; set; }

		// ✅ Navegaciones correctas
		public UserTeam UserTeam { get; set; }
		public Driver Driver { get; set; }
	}
}
