using F1.Data;
using F1.Helpers;
using F1.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;

#region PROCEDURES
/*
 CREATE OR ALTER VIEW V_RESULTRACE 
AS
	SELECT RR.POSITION_RACE, RR.LAP_TIME, D.DRIVER_NAME, T.TEAM_NAME, RR.POINTS, RR.ID_RACE
	FROM TEAM T
	INNER JOIN DRIVER D ON T.ID_TEAM = D.ID_TEAM
	INNER JOIN RESULT_RACE RR ON D.ID_DRIVER = RR.ID_DRIVER
	INNER JOIN RACE R ON RR.ID_RACE = R.ID_RACE
GO

CREATE OR ALTER VIEW V_USERTEAM
AS
	SELECT 
	CAST(ROW_NUMBER() OVER (ORDER BY ID_USER) AS INT) AS POSICION,
	UT.ID_USER, UT.ID_USER_TEAM, UT.USER_TEAM_NAME AS USER_TEAM_NAME, UT.TEAM_MONEY AS BUDGET, 
	D.DRIVER_NAME,	D.PRICE AS DRIVER_PRICE, D.DRIVER_IMG,
	T.TEAM_NAME AS TEAM, T.PRICE AS TEAM_PRICE, T.TEAM_IMG
	FROM USER_TEAM UT
	INNER JOIN DRIVER_USER_TEAM DUT ON UT.ID_USER_TEAM = DUT.ID_USER_TEAM 
	INNER JOIN DRIVER D ON DUT.ID_DRIVER = D.ID_DRIVER
	INNER JOIN TEAM T ON UT.ID_TEAM = T.ID_TEAM
	--WHERE UT.ID_USER_TEAM = 2
GO

CREATE VIEW V_LEAGUE
AS
	SELECT L.ID_LEAGUE, L.LEAGUE_NAME, UP.NICKNAME, UC.USER_TOTAL_POINTS 
	FROM LEAGUE L
	INNER JOIN USER_CLASSIFICATION UC ON L.ID_LEAGUE = UC.ID_LEAGUE
	INNER JOIN USER_PLAYER UP ON UC.ID_USER = UP.ID_USER
GO

CREATE VIEW V_USER_CLASSIFFICATION
AS
	SELECT CAST(ROW_NUMBER() OVER (ORDER BY ID_USER) AS INT) AS POSICION, ID_USER, USER_TOTAL_POINTS, ID_LEAGUE
	FROM USER_CLASSIFICATION
GO
 */
#endregion

namespace F1.Repositories
{
	public class RepositoryF1 : IRepositoryF1
	{
		#region INYECCION
		//INYECCION DEPENDENCIA CONTEXT
		private UsuariosContext context;
		#endregion

		#region CONSTRUCTOR
		//CONSTRUCTOR
		public RepositoryF1(UsuariosContext context)
		{
			this.context = context;
		}
		#endregion

		////GENERA EL SIGUIENTE REGISTRO DEL NUEVO PILOTO SELECCIONADO POR EL USUARIO PARA SU EQUIPO
		//#region MAX DRIVER USER TEAM
		//private async Task<int> MaximoDriverUserTeam()
		//{
		//    return this.context.DriverUserTeams.Max(x => x.IdDriverTeam) + 1;
		//}
		//#endregion

		//GENERA EL SIGUIENTE REGISTRO PARA EL ID DE LA LIGA
		#region MAX LEAGUE CODE
		private async Task<int> MaximoLeagueCode()
		{
			int? max = await this.context.Leagues.MaxAsync(x => (int?)x.LeagueCode);
			return (max ?? 0) + 1;
		}
		#endregion

		#region USUARIO
		//BUSCA AL USUARIO POR EL ID
		#region BUSCAR USUARIO
		public async Task<UserPlayer> FindUsuario(string email)
				{
					return await this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
				}
			#endregion

			//DEVUELVE EL ID MAX DE LA TABLA USER_PLAYER
			#region MAX USUARIO
			private int GetMaxIdUsuario()
				{
					if (this.context.Usuarios.Count() == 0)
					{
						return 1;
					}
					else
					{
						return this.context.Usuarios.Max(x => x.IdUser) + 1;
					}
				}
				#endregion

			#region REGISTRO USUARIO
			//METODO PARA CREAR AL USUARIO (REGISTRO), DONDE TAMBIEN SE ENCRIPTA LA PASSWORD
			public async Task RegisterUser(string nickname, string email, string password)
			{
				byte[] salt = HelperEncrypt.GenerateSalt();
				byte[] hash = HelperEncrypt.HashPassword(password, salt);

				UserPlayer user = new UserPlayer
				{
					//IdUser = this.GetMaxIdUsuario(),
					Nickname = nickname,
					Email = email,
					Salt = salt,
					PasswordSha = hash
				};
			
				context.Usuarios.Add(user);
				await context.SaveChangesAsync();
			}
			#endregion

		#region LOG IN
		//METODO PARA EL LOGIN DEL USUARIO DONDE COMPRUEBA EL CIFRADO DE AMBAS PASSWORD
		public async Task<UserPlayer> LogIn(string email, string password)
			{
			UserPlayer user = await this.FindUsuario(email);

			if (user == null)
				return null;

			bool valid = HelperEncrypt.VerifyPassword(password, user.PasswordSha, user.Salt);

			return valid ? user : null;
		}
			#endregion
		#endregion

		#region USERPAGE UPDATE
			#region UPDATE NICKNAME
			//METODO PARA CAMBIAR SOLAMENTE EL NICKNAME
			public async Task UpdateNickname(string nickname, string email)
			{
				UserPlayer user = await this.FindUsuario(email);
				user.Nickname = nickname;

				await this.context.SaveChangesAsync();
			}
			#endregion

			#region UPDATE EMAIL
			//METODO PARA CAMBIAR SOLAMENTE EL EMAIL
			public async Task UpdateEmail(string emailSession, string email)
			{
				UserPlayer user = await this.FindUsuario(email);
				user.Email = emailSession;

				await this.context.SaveChangesAsync();
			}
			#endregion

			#region UPDATE PASSWORD
			//METODO PARA CAMBIAR SOLAMENTE LA PASSWORD JUNTO AL SALT Y SU PASSWORD ENCRIPTADA
			public async Task UpdatePassword(string passSession, string email)
			{
				UserPlayer user = await this.FindUsuario(email);

				byte[] salt = HelperEncrypt.GenerateSalt();
				byte[] hash = HelperEncrypt.HashPassword(passSession, salt);

				user.Salt = salt;
				user.PasswordSha = hash;

				await context.SaveChangesAsync();
			}
			#endregion
			#endregion

        #region GETS
        //EXTRAE LOS PILOTOS
        #region GET DRIVERS
        public async Task<List<Driver>> GetDrivers()
			{
				return await this.context.Drivers.AsNoTracking().ToListAsync();
			}
        #endregion

        //EXTRAE LAS ESCUDERIAS (TEAMS)
        #region GET TEAMS
        public async Task<List<Team>> GetTeams()
			{
				return await this.context.Teams.AsNoTracking().ToListAsync();
			}
        #endregion

        //EXTRAE LAS CARRERAS
        #region GET RACES
        public async Task<List<Race>> GetRaces()
			{
				return await this.context.Races.AsNoTracking().ToListAsync();
			}
        #endregion

        //EXTRAE LAS CARRERAS REALIZADAS HASTA EL MOMENTO (A DIA DE HOY) Y AÑADIRA TAMBIEN LA PROXIMA CARRERA QUE SE REALIZARA
        #region GET RACES NOW
        public async Task<List<Race>> GetRacesNow()
			{
				//EXTRAIGO LAS CARRERAS
				List<Race> races = await this.GetRaces();

				//GENERO UNA LISTA AUXILIAR PARA IR INTRODUCIENDO LAS CARRERAS PASADAS Y LA ACTUAL A LA LISTA POR UN FILTRO DE TIEMPO (DIA ACTUAL)
				List<Race> racesView = new List<Race>();

				foreach (Race r in races)
				{
					DateTime dateRace = r.GpDateEnd;

					if (dateRace < DateTime.Now)
					{
						//AÑADE LAS CARRERAS QUE CUMPLEN EL FILTRO A LA LISTA AUXILIAR
						racesView.Add(r);
					}
					else
					{
						racesView.Add(r);
						break;
					}
				}

				return racesView.OrderByDescending(x => x.IdRace).ToList();
			}
        #endregion

        //EXTRAE LOS HORARIOS DE LAS CARRERAS
        #region GET SCHEDULE
        public async Task<List<Schedule>> GetSchedule()
			{
				return await this.context.Schedules.AsNoTracking().ToListAsync();
			}
        #endregion

        //EXTRAE TODOS LOS DATOS DEL EQUIPO DEL USUARIO (POSICION (KEY),ID_USER, ID_USER_TEAM, USER_TEAM_NAME, BUDGET, DRIVER_NAME, DRIVER_PRICE, DRIVER_IMG, TEAM, TEAM_PRICE Y TEAM_IMG)
        #region GET USER TEAMS VISTA
        public async Task<List<VistaUserTeam>> GetVistaUserTeams(int idUser)
		{
			return await this.context.VistaUserTeams.AsNoTracking().Where(x => x.IdUSer == idUser).ToListAsync();
		}
        #endregion

        //EXTRAE LOS DATOS SIMPLIFICADOS DEL EQUIPO DEL USUARIO (ID_USER_TEAM, USER_TEAM_NAME, TEAM_MONEY, ID_USER, ID_TEAM)
        #region GET USER TEAM
        public async Task<UserTeam> GetUserTeam(int idUser)
			{
				return await this.context.UserTeams.AsNoTracking().Where(x => x.IdUser == idUser).FirstAsync();
			}
        #endregion

        //EXTRAE LAS LIGAS
        #region GET LEAGUES
        public async Task<List<League>> GetLeagues()
			{
				return await this.context.Leagues.AsNoTracking().ToListAsync();
			}
        #endregion

        //EXTRAE TODAS LAS LIGAS CON SUS PUNTUACIONES Y USUARIOS (POSICION (KEY), ID_LEAGUE, LEAGUE_NAME, NICKNAME, USER_TOTAL_POINTS, ID_USER)
        #region GET VISTA LEAGUES
        public async Task<List<VistaLeague>> GetVistaLeague()
			{
				return await this.context.VistaLeagues.AsNoTracking().ToListAsync();
			}
        #endregion

        //EXTRAE LOS USUARIOS QUE ESTAN EN UNA LIGA CON SUS PUNTOS Y SU LIGA
        #region GET USER CLASSIFICATION
        public async Task<List<UserClassification>> GetUserClassification(int idUSer)
			{
				return await this.context.UserClassifications.AsNoTracking().Where(x => x.IdUser == idUSer).ToListAsync();
			}
        #endregion
        #endregion

        #region FINDS
        //BUSCA LOS RESULTADOS DE UNA CARRERA (ID_RACE) Y LOS ORDENA POR LA POSICION
        #region FIND RESULTS RACE
        public async Task<List<VistaResultRace>> FindResultsRace(int idRace)
			{
				return await this.context.VistaResultsRace.AsNoTracking().Where(x => x.IdRace == idRace).OrderBy(x => x.PositionRace).ToListAsync();
			}
        #endregion

        //BUSCA UNA CARRERA POR SU ID (ID_RACE)
        #region FIND RACE
        public async Task<Race> FindRace(int idRace)
			{
				return await this.context.Races.AsNoTracking().FirstOrDefaultAsync(x => x.IdRace == idRace);
			}
        #endregion

        //BUSCA EL EQUIPO DE UN USUARIO POR EL ID (ID_USER)
        #region FIND USER TEAMS
        public async Task<int> FindUserTeams(int idUser)
			{
				return await this.context.UserTeams.AsNoTracking().Where(x => x.IdUser == idUser).CountAsync();
			}
        #endregion

        //BUSCA EL EQUIPO DEL USUARIO POR EL ID DEL EQUIPO EN LA BBDD (ID_USER_TEAM)
        #region FIND DRIVER USER TEAM
        public async Task<List<DriverUserTeam>> FindDriverUserTeam(int idUSerTeam)
			{
				return await this.context.DriverUserTeams.AsNoTracking().Where(x => x.IdUserTeam == idUSerTeam).ToListAsync();
			}		
        #endregion

        //BUSCA SI EXISTE EL PILOTO EN EL EQUIPO
        #region FIND DRIVER USER TEAM SOBRECARGA POR ID
        public async Task<DriverUserTeam> FindDriverUserTeam(int idUSerTeam, int idDriver)
			{
				return await this.context.DriverUserTeams.AsNoTracking().FirstOrDefaultAsync(x => x.IdUserTeam == idUSerTeam && x.IdDriver == idDriver);
			}
        #endregion

        //EXTRAE LAS LIGAS EN LAS QUE ESTA EL USUARIO (ID_USER)
        #region FIND VISTA LEAGUE IDUSER
        public async Task<List<VistaLeague>> FindVistaLeague(int idUser)
			{
				return await this.context.VistaLeagues.AsNoTracking().Where(x => x.IdUSer == idUser).ToListAsync();
			}
        #endregion

        //EXTRAE LOS DATOS DE LOS USUARIOS QUE PERTENECEN A ESA LIGA (ID_LEAGUE) EN ORDEN DESCENDENTE POR PUNTOS TOTALES (USER_TOTAL_POINTS)
        #region FIND VISTA LEAGUE IDLEAGUE
        public async Task<List<VistaLeague>> FindVistaLeagueMembers(int idLeague)
			{
				return await this.context.VistaLeagues.AsNoTracking().Where(x => x.IdLeague == idLeague).OrderByDescending(x => x.UserTotalPoints).ToListAsync();
			}
			#endregion

		//BUSCA LA LIGA POR EL NOMBRE DE DICHA LIGA
		#region FIND VISTA LEAGUE NAME
			public async Task<List<VistaLeague>> FindVistaLeagueName(string leagueName)
			{
				return await this.context.VistaLeagues.AsNoTracking().Where(x => x.LeagueName == leagueName).ToListAsync();
			}
        #endregion

        //BUSCA TODAS LAS LIGAS DEL USUARIO (ID_LEAGUE, ID_USER)
        #region FIND USER IN LEAGUE
        public async Task<List<VistaLeague>> FindUserInLeague(int idLeague, int idUser)
			{
				return await this.context.VistaLeagues.AsNoTracking().Where(x => x.IdLeague == idLeague && x.IdUSer == idUser).ToListAsync();
			}
        #endregion

        //BUSCA AL PILOTO EN UN EQUIPO (ID_USER_TEAM, ID_DRIVER)
        #region FIND DRIVER IN TEAM
        public async Task<List<DriverUserTeam>> FindDriverInTeamPlayer(int idUserTeam, int idDriver)
			{
				return await this.context.DriverUserTeams.AsNoTracking().Where(x => x.IdUserTeam == idUserTeam && x.IdDriver == idDriver).ToListAsync();
			}
        #endregion

        //BUSCA UNA LIGA POR EL CODIGO DE LA LIGA (LEAGUE_CODE)
        #region FIND LEAGUE CODE
        public async Task<League> FindLeagueCode(int leagueCode)
			{
				return await this.context.Leagues.AsNoTracking().FirstOrDefaultAsync(x => x.LeagueCode == leagueCode);
			}
        #endregion

        //BUSCA LA LIGA POR EL ID DE LA LIGA (ID_LEAGUE)
        #region FIND LEAGUE IDLEAGUE
        public async Task<League> FindLeague(int idLeague)
			{
				return await this.context.Leagues.AsNoTracking().FirstOrDefaultAsync(x => x.IdLeague == idLeague);
			}
        #endregion

        //BUSCA UN PILOTO POR EL ID (ID_DRIVER)
        #region FIND DRIVER
        public async Task<Driver> FindDriver(int idDriver)
			{
				return await this.context.Drivers.AsNoTracking().FirstOrDefaultAsync(x => x.IdDriver == idDriver);
			}
        #endregion

        //BUSCA UN PILOTO EN LOS RESULTADOS DE UNA CARRERA (ID_RACE, ID_DRIVER)
        #region FIND DRIVER IN RESULT RACE
        public async Task<ResultRace> FindResultRaceDriver(int idRace, int idDriver)
			{
				return await this.context.ResultRaces.AsNoTracking().FirstOrDefaultAsync(x => x.IdRace == idRace && x.IdDriver == idDriver);
			}
        #endregion

        //BUSCA UN USUARIO EN LA LIGA DE LA CLASIFICACION (ID_USER, ID_LEAGUE)
        #region VISTA DE UN SOLO USER CLASSIFICATION
        public async Task<VistaUserClassification> FindUserClassification(int idUser, int idLeague)
			{
			return await this.context.VistaUserClassifications.AsNoTracking().FirstOrDefaultAsync(x => x.IdUser == idUser && x.IdLeague == idLeague);
			}
		#endregion
		#endregion

		#region CREATES
		//CREA UN REGISTRO DE LA CARRERA INDICADA (POSITION, POINTS, LAP_TIME, ID_RACE, ID_DRIVER)
		#region CREATE RESULTS RACE
		public async Task CreateResultRace(int position, int points, string lapTime, int idRace, int idDriver)
		{
			if (!TimeSpan.TryParse(lapTime, out TimeSpan parsedLapTime))
				throw new ArgumentException("Formato de lapTime incorrecto. Debe ser hh:mm:ss o mm:ss");

			ResultRace resultRace = new ResultRace
			{
				PositionRace = position,
				Points = points,
				LapTime = parsedLapTime,
				IdRace = idRace,
				IdDriver = idDriver
			};

			this.context.ResultRaces.Add(resultRace);
			await this.context.SaveChangesAsync();
		}
		#endregion

		//CREA UN EQUIPO AL USUARIO (USER_TEAM_NAME, ID_USER, ID_TEAM)
		#region CREATE USER TEAM
		public async Task CreateUserTeam(string userTeamName, int idUser, int idTeam)
			{
				UserTeam userTeam = new UserTeam
				{
					UserTeamName = userTeamName,
					TeamMoney = Decimal.Parse("100"),
					IdUser = idUser,
					IdTeam = idTeam
				};

				this.context.UserTeams.Add(userTeam);

				await this.context.SaveChangesAsync();
			}
			#endregion

			//AÑADE EL EQUIPO AL EQUIPO DEL USUARIO (ID_TEAM_USER, ID_TEAM)
			#region CREATE USER TEAM
			public async Task CreateUserTeam(int idTeamUser, int idTeam)
			{
				UserTeam UserTeam = new UserTeam
				{
					//IdUserTeam = idTeamUser,
					IdTeam = idTeam
				};

				this.context.UserTeams.Add(UserTeam);

				await this.context.SaveChangesAsync();
			}
			#endregion

			//AÑADE UN PILOTO AL EQUIPO DEL USUARIO (ID_TEAM_USER, ID_DRIVER)
			#region CREATE USER TEAM DRIVERS
			public async Task CreateUserTeamDriver(int idTeamUser, int idDriver)
			{
			DriverUserTeam driverUserTeam = new DriverUserTeam
			{
				IdUserTeam = idTeamUser,
				IdDriver = idDriver
			};

			this.context.DriverUserTeams.Add(driverUserTeam);

				await this.context.SaveChangesAsync();
			}
			#endregion

			//AÑADE UN USUARIO A UNA LIGA (ID_USER, ID_LEAGUE)
			#region CREATE/INSERT USER IN LEAGUE
			public async Task InsertUserLeague(int idUser, int idLeague)
			{
				UserClassification userClassification = new UserClassification
				{
					IdUser = idUser,
					UserTotalPoints = 0,
					IdLeague = idLeague
				};

				await this.context.UserClassifications.AddAsync(userClassification);

				await this.context.SaveChangesAsync();
			}
			#endregion

			//CREA UNA LIGA (ID_USER, LEAGUE_NAME)
			#region CREATE LEAGUE
			public async Task CreateLeague(int idUser, string leagueName)
			{
				League league = new League
                {
					LeagueName = leagueName,
					LeagueCode = await this.MaximoLeagueCode()
				};

				await this.context.Leagues.AddAsync(league);

				await this.context.SaveChangesAsync();

				await this.InsertUserLeague(idUser, league.IdLeague);
			}
			#endregion

			//AÑADE LOS PUNTOS A LOS PILOTOS Y A LAS ESCUDERIAS (ID_RACE)
			#region CREATE POINTS DRIVER
			public async Task InsertPointsDriver(int idRace)
			{
				List<Driver> drivers = await this.context.Drivers.ToListAsync();

				List<Team> teams = await this.context.Teams.ToListAsync();

				await this.RestartPointsTeam();

				foreach (Driver d in drivers)
				{
					ResultRace resultRace = await this.FindResultRaceDriver(idRace, d.IdDriver);

					if (d.IdDriver == resultRace.IdDriver)
					{
						Driver driverUpdate = await this.FindDriver(d.IdDriver);
						driverUpdate.TotalPoints = d.TotalPoints + resultRace.Points;

						foreach(Team t in teams)
						{
							if(d.IdTeam == t.IdTeam)
							{
								t.TotalPoints = t.TotalPoints + d.TotalPoints;
							}
						}

						await this.context.SaveChangesAsync();
					}
				}
			}
			#endregion

			//AÑADE LOS PUNTOS A LOS EUIPOS DE LAS LIGAS INDIFERENTEMENTE A LOS PUNTOS DE LOS PILOTOS O DE LAS ESCUDERIAS (ID_USER, ID_LEAGUE, ID_RACE, ID_USER_TEAM)
			#region CREATE/INSERT POINTS USER CLASSIFICATION
			public async Task InsertPointsUserClassification(int idUSer, int idLeague, int idRace, int idUserTeam)
			{
				//LISTA DE LOS PILOTOS DEL EQUIPO POR ID DE EQUIPO
				List<DriverUserTeam> driversTeam = await this.FindDriverUserTeam(idUserTeam);

				UserTeam userteam = await this.GetUserTeam(idUSer);

				List<Driver> drivers = await this.context.Drivers.ToListAsync();

				List<Team> teams = await this.context.Teams.ToListAsync();

				int sumaPuntos = 0;

				//SUMA LOS PUNTOS CONSEGUIDOS POR LO PILOTOS Y LOS AÑADE AL EQUIPO DE LA LIGA DE DICHO USUARIO
				foreach (DriverUserTeam dT in driversTeam)
				{
					ResultRace resultRace = await this.FindResultRaceDriver(idRace, dT.IdDriver);

					if (dT.IdDriver == resultRace.IdDriver)
					{
						sumaPuntos += resultRace.Points;
					}
				}

				//SUMA LOS PUNTOS DE LOS DOS PILOTOS DEL EQUIPO ESCOGIDO EN SU EQUIPO DE LA LIGA
				foreach (Driver d in drivers)
				{
					ResultRace resultRace = await this.FindResultRaceDriver(idRace, d.IdDriver);

					if (d.IdDriver == resultRace.IdDriver)
					{
						foreach (Team t in teams)
						{
							if (d.IdTeam == t.IdTeam && userteam.IdTeam == t.IdTeam)
							{
								sumaPuntos += resultRace.Points;
							}
						}

						await this.context.SaveChangesAsync();
					}
				}

				//AÑADE LOS PUNTOS A LOS EQUIPOS DE LAS LIGAS
				VistaUserClassification userClassification = await this.FindUserClassification(idUSer, idLeague);
				userClassification.UserTotalPoints = userClassification.UserTotalPoints + sumaPuntos;

				//AÑADE LOS PUNTOS DE LOS PILOTOS Y SUS EQUIPOS
				await this.InsertPointsDriver(idRace);

                await this.context.SaveChangesAsync();
			}
			#endregion
        #endregion

        #region REMOVE
        //ELIMINA UN PILOTO DEL EQUIPO DEL USUARIO EN CUESTION (ID_TEAM_USER, ID_DRIVER)
        #region REMOVE USER TEAM DRIVER
        public async Task RemoveUserTeamDriver(int idTeamUser, int idDriver)
			{
				DriverUserTeam driverUserTeam = await this.FindDriverUserTeam(idTeamUser, idDriver);
				
				if(driverUserTeam != null)
				{
					this.context.DriverUserTeams.Remove(driverUserTeam);
					this.context.SaveChangesAsync();
				}
			}
        #endregion

        //ELIMINA EL EQUIPO DEL USUARIO EN CUESTION (NO SE ESTA USANDO) (ID_TEAM_USER)
        #region REMOVE USER TEAM
        public async Task RemoveUserTeam(int idTeamUser)
			{
				UserTeam UserTeam = new UserTeam
				{
					IdUserTeam = idTeamUser
				};

				if (this.context.UserTeams.Contains(UserTeam) == true)
				{
					this.context.UserTeams.Remove(UserTeam);
				}

				await this.context.SaveChangesAsync();
			}
        #endregion

        //ELIMINA A UN USUARIO DE LA LIGA EN CUESTION (ID_USER, ID_LEAGUE)
        #region REMOVE USER LEAGUE
        public async Task RemoveUserLeague(int idUser, int idLeague)
		{
			UserClassification userClassification = await this.context.UserClassifications.FirstOrDefaultAsync(x => x.IdUser == idUser && x.IdLeague == idLeague);

			if (userClassification != null)
			{
				this.context.UserClassifications.Remove(userClassification);
				await this.context.SaveChangesAsync();
			}
		}
        #endregion

        //ELIMINA LOS PUNTOS DE LOS PILOTOS DE UNA CARRERA EN CONCRETO (ID_RACE)
        #region REMOVE POINTS DRIVER
        public async Task RemovePointsDriver(int idRace)
			{
				List<Driver> drivers = await this.context.Drivers.ToListAsync();

				List<Team> teams = await this.context.Teams.ToListAsync();

				foreach (Driver d in drivers)
				{
					ResultRace resultRace = await this.FindResultRaceDriver(idRace, d.IdDriver);

					if (d.IdDriver == resultRace.IdDriver)
					{
						Driver driverUpdate = await this.FindDriver(d.IdDriver);
						driverUpdate.TotalPoints = d.TotalPoints - resultRace.Points;

						foreach (Team t in teams)
						{
							if (d.IdTeam == t.IdTeam)
							{
								t.TotalPoints = t.TotalPoints - resultRace.Points;
							}
						}

						await this.context.SaveChangesAsync();
					}
				}

			}
        #endregion

        //ELIMINA UN REGISTRO DEL RESULTADO DE CARRERA
        #region DELETE RESULT RACE
        public async Task RemoveResultRace(int idRace, int idDriver)
			{
				ResultRace driverResultRace = await this.FindResultRaceDriver(idRace, idDriver);

				this.context.ResultRaces.Remove(driverResultRace);

				await this.context.SaveChangesAsync();
			}
        #endregion
        #endregion

        #region UPDATES
			//RESTABLECE LOS PUNTOS DE LOS PILOTOS A CERO
			#region UPDATE/RESTART POINTS DRIVER
			public async Task RestartPointsDriver()
			{
				//RESTABLECE LOS PUNTOS DE LOS PILOTOS
				List<Driver> drivers = await this.context.Drivers.ToListAsync();

			foreach (Driver d in drivers)
				{
					d.TotalPoints = 0;

					await this.context.SaveChangesAsync();
				}

				//RESTABLECE LOS PUNTOS DE LOS EQUIPOS
				List<Team> teams = await this.context.Teams.ToListAsync();

			foreach (Team t in teams)
				{
					t.TotalPoints = 0;

					await this.context.SaveChangesAsync();
				}

				//RESTABLECE LOS PUNTOS DE LAS LIGAS
				List<VistaLeague> userPointsLeague = await this.GetVistaLeague();

				foreach (VistaLeague viewLeague in userPointsLeague)
				{
					viewLeague.UserTotalPoints = 0;

					await this.context.SaveChangesAsync();
				}
			}
			#endregion

			//RESTABLECE LOS PUNTOS DE LAS ESCUDERIAS A CERO
			#region UPDATE/RESTART POINTS TEAM
			public async Task RestartPointsTeam()
			{
				List<Team> teams = await this.context.Teams.ToListAsync();

			foreach (Team t in teams)
				{
					t.TotalPoints = 0;

					await this.context.SaveChangesAsync();
				}
			}
			#endregion

			//ACTUALIZA LA ESCUDERIA AL EQUIPO DEL USUARIO EN CUESTION (ID_USER, ID_TEAM_USER, ID_TEAM)
			#region UPDATE TEAM USER PLAYER
			public async Task UpdateTeamUserPlayer(int idUser, int idTeamUser, int idTeam)
			{
				UserTeam userTeam = await this.GetUserTeam(idUser);

				if (userTeam != null)
				{
					userTeam.IdTeam = idTeam;
					await this.context.SaveChangesAsync();
				}
			}
		#endregion

			//ACTUALIZA UN REGISTRO DEL RESULTADO DE LA CARRERA
			#region UPDATE RESULT RACE
			public async Task UpdateResultRace(int position, int points, string lapTime, int idRace, int idDriver)
			{
				if (!TimeSpan.TryParse(lapTime, out TimeSpan parsedLapTime))
					throw new ArgumentException("Formato de lapTime incorrecto. Debe ser hh:mm:ss o mm:ss");

				ResultRace newResultRace = new ResultRace
				{
					PositionRace = position,
					Points = points,
					LapTime = parsedLapTime,
					IdRace = idRace,
					IdDriver = idDriver,
				};

				this.context.ResultRaces.Add(newResultRace);
				await this.context.SaveChangesAsync();
			}
			#endregion
		#endregion

		#region PAGINACION LEAGUES
		public async Task<PaginacionLeagues> PaginacionLeaguesAsync(int posicion)
        {
            List<League> leagues = await this.GetLeagues();
            int numregistros = leagues.Count;

            List<League> listaitemspaginados = leagues.Skip(posicion).Take(3).ToList();

            PaginacionLeagues itemsPaginados = new PaginacionLeagues
            {
                Leagues = listaitemspaginados,
                NumRegistros = numregistros
            };

            return itemsPaginados;
        }
		#endregion
	}
}
