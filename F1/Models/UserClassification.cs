using System.ComponentModel.DataAnnotations.Schema;

namespace F1.Models
{
	[Table("USER_CLASSIFICATION")]
	public class UserClassification
	{
		[Column("ID_USER")]
		public int IdUser { get; set; }

		[Column("ID_LEAGUE")]
		public int IdLeague { get; set; }

		[Column("USER_TOTAL_POINTS")]
		public int UserTotalPoints { get; set; }

		public UserPlayer User { get; set; }
		public League League { get; set; }
	}
}
