using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F1.Models
{
	[Table("RESULT_RACE")]
	public class ResultRace
	{
		[Key]
		[Column("ID_RESULT_RACE")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int IdResultRace { get; set; }

		[Required]
		[Column("POSITION_RACE")]
		public int PositionRace { get; set; }

		[Required]
		[Column("POINTS")]
		public int Points { get; set; }

		[Required]
		[Column("LAP_TIME")]
		public TimeSpan LapTime { get; set; }

		[Required]
		[Column("ID_RACE")]
		public int IdRace { get; set; }

		[Required]
		[Column("ID_DRIVER")]
		public int IdDriver { get; set; }
		public Driver Driver { get; internal set; }
		public Race Race { get; internal set; }
	}
}
