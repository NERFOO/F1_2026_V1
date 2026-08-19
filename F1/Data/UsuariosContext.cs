using F1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace F1.Data
{
	public class UsuariosContext : IdentityDbContext<IdentityUser>
	{
		public UsuariosContext(DbContextOptions<UsuariosContext> options)
			: base(options)
		{ }

		// Tus tablas existentes
		public DbSet<UserPlayer> Usuarios { get; set; }
		public DbSet<Driver> Drivers { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<Race> Races { get; set; }
		public DbSet<ResultRace> ResultRaces { get; set; }
		public DbSet<VistaResultRace> VistaResultsRace { get; set; }
		public DbSet<Schedule> Schedules { get; set; }
		public DbSet<VistaUserTeam> VistaUserTeams { get; set; }
		public DbSet<UserTeam> UserTeams { get; set; }
		public DbSet<DriverUserTeam> DriverUserTeams { get; set; }
		public DbSet<League> Leagues { get; set; }
		public DbSet<UserClassification> UserClassifications { get; set; }
		public DbSet<VistaUserClassification> VistaUserClassifications { get; set; }
		public DbSet<VistaLeague> VistaLeagues { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder); // 🔑 Obligatorio para Identity

			// =========================
			// CLAVES COMPUESTAS
			// =========================
			modelBuilder.Entity<ResultRace>()
				.HasKey(r => new { r.IdRace, r.IdDriver });

			modelBuilder.Entity<UserClassification>()
				.HasKey(x => new { x.IdUser, x.IdLeague });

			modelBuilder.Entity<DriverUserTeam>()
				.HasKey(d => new { d.IdUserTeam, d.IdDriver });

			// =========================
			// RELACIONES
			// =========================
			modelBuilder.Entity<UserClassification>()
				.HasOne(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.IdUser);

			modelBuilder.Entity<UserClassification>()
				.HasOne(x => x.League)
				.WithMany()
				.HasForeignKey(x => x.IdLeague);

			modelBuilder.Entity<Driver>()
				.HasOne(d => d.Team)
				.WithMany()
				.HasForeignKey(d => d.IdTeam);

			modelBuilder.Entity<ResultRace>()
				.HasOne(r => r.Driver)
				.WithMany()
				.HasForeignKey(r => r.IdDriver);

			modelBuilder.Entity<ResultRace>()
				.HasOne(r => r.Race)
				.WithMany()
				.HasForeignKey(r => r.IdRace);

			modelBuilder.Entity<UserTeam>()
				.HasOne(u => u.User)
				.WithMany()
				.HasForeignKey(u => u.IdUser);

			modelBuilder.Entity<UserTeam>()
				.HasOne(u => u.Team)
				.WithMany()
				.HasForeignKey(u => u.IdTeam);

			modelBuilder.Entity<DriverUserTeam>()
				.HasOne(d => d.UserTeam)
				.WithMany(u => u.DriverUserTeams)
				.HasForeignKey(d => d.IdUserTeam);

			modelBuilder.Entity<DriverUserTeam>()
				.HasOne(d => d.Driver)
				.WithMany()
				.HasForeignKey(d => d.IdDriver);

			// =========================
			// VISTAS (SOLO LECTURA)
			// =========================
			modelBuilder.Entity<VistaResultRace>()
				.HasNoKey()
				.ToView("V_RESULTRACE");

			modelBuilder.Entity<VistaUserTeam>()
				.HasNoKey()
				.ToView("V_USERTEAM", "dbo");

			modelBuilder.Entity<VistaUserClassification>()
				.HasNoKey()
				.ToView("V_USER_CLASSIFICATION");

			modelBuilder.Entity<VistaLeague>()
				.HasNoKey()
				.ToView("V_LEAGUE");
		}
	}
}