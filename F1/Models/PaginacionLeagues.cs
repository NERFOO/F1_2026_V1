using System.ComponentModel.DataAnnotations.Schema;

namespace F1.Models
{
	public class PaginacionLeagues
	{
		public List<League> Leagues { get; set; }

		public int NumRegistros { get; set; }
	}
}
