using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F1.Models
{
	[Table("DRIVER")]
	public class Driver
	{
		[Key]
		[Column("ID_DRIVER")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int IdDriver { get; set; }

		[Required]
		[Column("DRIVER_NAME")]
		public string DriverName { get; set; }

		[Required]
		[Column("NACIONALITY")]
		public string Nacionality { get; set; }

		[Required]
		[Column("PRICE")]
		public decimal Price { get; set; }

		[Required]
		[Column("TOTAL_POINTS")]
		public int TotalPoints { get; set; }

		[Required]
		[Column("ID_TEAM")]
		public int IdTeam { get; set; }

		[Required]
		[Column("DRIVER_IMG")]
		public string DriverImg { get; set; }
		public Team Team { get; set; }
	}
}
