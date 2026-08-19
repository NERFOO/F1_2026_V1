using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F1.Models
{
	[Table("USER_PLAYER")]
	public class UserPlayer
	{
		[Key]
		[Column("ID_USER")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int IdUser { get; set; }

		[Column("NICKNAME")]
		public string Nickname { get; set; }

		[Column("USER_EMAIL")]
		public string Email { get; set; }

		[Column("USER_PASSWORD_SHA")]
		public byte[] PasswordSha { get; set; }

		[Column("SALT")]
		public byte[] Salt { get; set; }
	}
}
