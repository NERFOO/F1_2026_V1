using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F1.Models
{
	[Table("SCHEDULE")]
	public class Schedule
	{
		[Key]
		[Column("ID_SCHEDULE")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int IdSchedule { get; set; }

		[Required]
		[Column("SCHEDULE_NAME")]
		public string ScheduleName { get; set; }

		[Required]
		[Column("SCHEDULE_DAY")]
		public string ScheduleDay { get; set; }

		[Required]
		[Column("SCHEDULE_TIME")]
		public TimeSpan ScheduleTime { get; set; }

		[Required]
		[Column("ID_RACE")]
		public int IdRace { get; set; }
	}
}
