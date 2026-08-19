using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F1.Models
{
	[Table("RACE")]
	public class Race
	{
		[Key]
		[Column("ID_RACE")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int IdRace { get; set; }

		[Required]
		[Column("GP_NAME")]
		public string GpName { get; set; }

		[Required]
		[Column("CIRCUIT_NAME")]
		public string CircuitName { get; set; }

		[Required]
		[Column("COUNTRY")]
		public string Country { get; set; }

		[Required]
		[Column("GP_DATE_START")]
		public DateTime GpDateStart { get; set; }

		[Required]
		[Column("GP_DATE_END")]
		public DateTime GpDateEnd { get; set; }

		[Required]
		[Column("GP_LENGTH")]
		public int GpLength { get; set; }

		[Required]
		[Column("GP_DISTANCE")]
		public decimal GpDistance { get; set; }

		[Required]
		[Column("TURN")]
		public int Turn { get; set; }

		[Required]
		[Column("LAP")]
		public int Lap { get; set; }

		[Required]
		[Column("LAST_WINNER")]
		public string LastWinner { get; set; }

		[Required]
		[Column("FAST_LAP")]
		public string FastLap { get; set; }

		[Required]
		[Column("GP_IMG")]
		public string GpImg { get; set; }
	}
}
