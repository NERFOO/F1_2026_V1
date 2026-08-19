using F1.Extensions;
using F1.Filters;
using F1.Models;
using F1.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Rules;
using System.Diagnostics;

namespace F1.Controllers
{
	public class F1Controller : Controller
	{
		#region INYECCION
		//INYECCION DEPENDENCIA REPOSITORIO
		private IRepositoryF1 repo;
		#endregion

		#region CONSTRUCTOR
		//CONSTRUCTOR
		public F1Controller(IRepositoryF1 repo)
		{
			this.repo = repo;
		}
		#endregion

		#region REGISTRO
		//ACCION PARA EL REGISTRO DEL USUARIO
		public async Task<IActionResult> Register()
		{
			return View();
		}

		//ACCION DE ENVIO DE DATOS Y COMPROBACION DEL EMAIL (EXISTENTE) PARA EL REGISTRO DEL USUARIO
		[HttpPost]
		public async Task<IActionResult> Register(string nickname, string email, string password)
		{
			UserPlayer usuario = await this.repo.FindUsuario(email);

			if (usuario == null)
			{
				//CREAMOS EL USUARIO
				await this.repo.RegisterUser(nickname, email, password);

				////BUSCAMOS EL USUARIO
				//UserPlayer user = await this.repo.FindUsuario(email);

				////Y SE GUARDA EN SESSION
				//HttpContext.Session.SetObject("Usuario", user);
			}
			else
			{
				return View(ViewData["UsuarioYaRegistrado"] = "The email has already been registered");
			}

			return RedirectToAction("LogIn", "Managed");
		}
		#endregion

		#region LOG IN
		//ACCION PARA EL LOGIN DEL USUARIO
		public async Task<IActionResult> LogIn()
		{
			return View();
		}

		//ACCION PARA ENVIAR Y COMPROBAR EL LOGIN DEL USUARIO
		[HttpPost]
		public async Task<IActionResult> LogIn(string email, string password)
		{
			UserPlayer user = await this.repo.LogIn(email, password);

			if (user == null)
			{
				return View(ViewData["ErrorLogin"] = "Credentials don't match");
			}
			else
			{
				//SI EL USUARIO INTRODUCIDO ES EL ADMIN LE REDIRECCIONA A SU PAGINA
				if (user.Email == "adminpr0yect0F1@admin.es")
				{
					//GUARDAMOS EL ADMIN EN SESSION
					HttpContext.Session.SetObject("Admin", user);

					return RedirectToAction("Index");
				}
				else
				{
					//GUARDAMOS EL USUARIO EN SESSION
					HttpContext.Session.SetObject("Usuario", user);

					//return View(ViewData["User"] = user);
					return RedirectToAction("Index");
				}
			}
		}
		#endregion

		#region INDEX
		//ACCION PARA LA PAGINA INICIAL
		public async Task<IActionResult> Index()
		{
			List<Race> raceNow = await this.repo.GetRacesNow();

			List<Schedule> schedule = await this.repo.GetSchedule();
			ViewData["Schedule"] = schedule;

			return View(raceNow);
		}
		#endregion

		[AuthorizeUsers]
		#region USERPAGE
		//ACCION DEL USUARIO CON SUS DATOS PARA LA EDICION
		public async Task<IActionResult> UserPage(string? logout, string? nickname, string? email, string? password)
		{

			if (logout != null)
			{
				HttpContext.Session.Remove("Usuario");
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

				return RedirectToAction("Index");
			}

			if (nickname == "nickname")
			{
				ViewData["nickname"] = "";
			}
			else
			{
				ViewData["nickname"] = "none";
			}

			if (email == "email")
			{
				ViewData["email"] = "";
			}
			else
			{
				ViewData["email"] = "none";
			}

			if (password == "password")
			{
				ViewData["password"] = "";
			}
			else
			{
				ViewData["password"] = "none";
			}

			UserPlayer usuario = HttpContext.Session.GetObject<UserPlayer>("Usuario");

			return View(usuario);
		}

		//METODO POST PARA CAMBIAR CUALQUIERA DE SUS DATOS INDEPENDIENTEMENTE CON DISPLAY NONE EN LOS FORMS
		[HttpPost]
		public async Task<IActionResult> UserPage(string? nickname, string? email, string? password)
		{
			if (nickname != null)
			{
				UserPlayer user = HttpContext.Session.GetObject<UserPlayer>("Usuario");

				await this.repo.UpdateNickname(nickname, user.Email);
			}
			if (email != null)
			{
				UserPlayer user = HttpContext.Session.GetObject<UserPlayer>("Usuario");

				await this.repo.UpdateEmail(email, user.Email);
			}
			if (password != null)
			{
				UserPlayer user = HttpContext.Session.GetObject<UserPlayer>("Usuario");

				await this.repo.UpdatePassword(password, user.Email);
			}

			ViewData["CambioUser"] = "Los cambios no se veran hasta el proximo inicio de sesion";
			ViewData["nickname"] = "none";
			ViewData["email"] = "none";
			ViewData["password"] = "none";

			UserPlayer usuario = HttpContext.Session.GetObject<UserPlayer>("Usuario");
			return View(usuario);
		}
		#endregion

		[Authorize(Roles = "Admin")]
		#region ADMINPAGE
		//ACCION PARA MOSTRAR LAS OPCIONES QUE TIENE EL ADMIN (PANEL DE DISTRIBUCION)
		public async Task<IActionResult> AdminPage(string? logout)
		{
			if (logout != null)
			{
				HttpContext.Session.Remove("Admin");
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

				return RedirectToAction("Index");
			}

			return View();
		}
		#endregion

		#region GAME RULES
		//ACCION DONDE SE MUESTRAN LAS REGLAS DEL JUEGOS Y SUS PUNCUACIONES
		public async Task<IActionResult> GameRules()
		{
			return View();
		}
		#endregion

		#region HOW TO PLAY
		//ACCION DONDE SE MUESTRA LA JUGABILIDAD
		public async Task<IActionResult> HowToPlay()
		{
			return View();
		}
		#endregion

		[AuthorizeUsers]
		#region TEAM PAGE
		//ACCION DONDE SE MUESTRAN LOS EQUIPOS DEL USUARIO
		public async Task<IActionResult> TeamPage()
		{
			int idUser = HttpContext.Session.GetObject<UserPlayer>("Usuario").IdUser;

			List<VistaUserTeam> userTeams = await this.repo.GetVistaUserTeams(idUser);

			int userTeamCount = await this.repo.FindUserTeams(idUser);

			if (userTeamCount == 0)
			{
				return RedirectToAction("CreateUserTeam");
			}

			UserTeam userTeam = await this.repo.GetUserTeam(idUser);
			ViewData["UserTeam"] = userTeam;

			List<Team> teams = await this.repo.GetTeams();
			ViewData["Teams"] = teams;


			//CONDICION PARA PODER MODIFICAR EL EQUIPO O NO

			// EXTRAIGO LAS CARRERAS
			List<Race> racesNow = await this.repo.GetRacesNow();

			foreach (Race r in racesNow)
			{
				DateTime dateRaceStart = r.GpDateStart;
				DateTime dateRaceEnd = r.GpDateEnd;
				DateTime today = DateTime.Now;

				if (today >= dateRaceStart && today <= dateRaceEnd)
				{
					ViewData["ManageTeam"] = "Unable to apply changes";
					return View(userTeams);
				}
			}
			ViewData["ManageTeam"] = "Manage";
			return View(userTeams);
		}
		#endregion

		[Authorize(Roles = "Admin")]
		#region CREAR REGISTRO DE CARRERA
		//VISTA PARA CREAR UN RESULTADO DE CARRERA
		public async Task<IActionResult> CreateResultRace()
		{
			List<Driver> drivers = await this.repo.GetDrivers();
			ViewData["Drivers"] = drivers;
			List<Race> races = await this.repo.GetRaces();
			ViewData["Races"] = races;

			return View();
		}

		//ACCION PARA CREAR EL REGISTRO DE CARRERA
		[HttpPost]
		public async Task<IActionResult> CreateResultRace(int position, int points, string lapTime, int idRace, int idDriver)
		{
			List<Driver> drivers = await this.repo.GetDrivers();
			ViewData["Drivers"] = drivers;
			List<Race> races = await this.repo.GetRaces();
			ViewData["Races"] = races;

			await this.repo.CreateResultRace(position, points, lapTime, idRace, idDriver);

			ViewData["ResultRaceCreation"] = "Has been successfully added";

			return View();
		}
        #endregion

        [Authorize(Roles = "Admin")]
        #region ACTUALIZAR REGISTRO DE CARRERA
        //VISTA PARA ACTUALIZAR UN RESULTADO DE LA CARRERA
        public async Task<IActionResult> UpdateResultRace()
        {
            List<Driver> drivers = await this.repo.GetDrivers();
            ViewData["Drivers"] = drivers;
            List<Race> races = await this.repo.GetRaces();
            ViewData["Races"] = races;

			ViewData["ResultRace"] = "";

            return View();
        }

        //ACCION PARA ACTUALIZAR EL REGISTRO DE LA CARRERA
        [HttpPost]
        public async Task<IActionResult> UpdateResultRace(int? position, int? points, string? lapTime, int? idRace, int? idDriver)
        {
            List<Driver> drivers = await this.repo.GetDrivers();
            ViewData["Drivers"] = drivers;
            List<Race> races = await this.repo.GetRaces();
            ViewData["Races"] = races;


			if(position != null && points != null && lapTime != null && idRace != null && idDriver != null)
			{
				await this.repo.UpdateResultRace(position.Value, points.Value, lapTime, idRace.Value, idDriver.Value);

				ViewData["ResultRaceCreation"] = "Has been successfully updated";
			}
			else if(idRace != null && idDriver != null)
			{
                ResultRace result = await this.repo.FindResultRaceDriver(idRace.Value, idDriver.Value);
                ViewData["ResultRace"] = result;

				await this.repo.RemoveResultRace(idRace.Value, idDriver.Value);
            }

            return View();
        }
        #endregion

        [Authorize(Roles = "Admin")]
        #region ELIMINAR REGISTRO DE CARRERA
        //VISTA PARA ELIMINAR UN RESULTADO DE LA CARRERA
        public async Task<IActionResult> DeleteResultRace()
        {
            List<Driver> drivers = await this.repo.GetDrivers();
            ViewData["Drivers"] = drivers;
            List<Race> races = await this.repo.GetRaces();
            ViewData["Races"] = races;

            return View();
        }

        //ACCION PARA ELIMINAR EL REGISTRO DE LA CARRERA
        [HttpPost]
        public async Task<IActionResult> DeleteResultRace(int idRace, int idDriver)
        {
            List<Driver> drivers = await this.repo.GetDrivers();
            ViewData["Drivers"] = drivers;
            List<Race> races = await this.repo.GetRaces();
            ViewData["Races"] = races;

            await this.repo.RemoveResultRace(idRace, idDriver);

            ViewData["ResultRaceCreation"] = "Has been successfully deleted";

            return View();
        }
        #endregion

        #region RESULTS RACE
        //MUESTRA LOS REGISTROS DE UNA CARRERA
        public async Task<IActionResult> ViewResultRace()
		{
			List<Race> races = await this.repo.GetRaces();
			ViewData["Races"] = races;

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ViewResultRace(int idRace)
		{
			List<Race> races = await this.repo.GetRaces();
			ViewData["Races"] = races;

			ViewData["IdRace"] = idRace;

			List<VistaResultRace> resultRace = await this.repo.FindResultsRace(idRace);
			return View(resultRace);
		}
		#endregion

		[AuthorizeUsers]
		#region CREATE USER TEAM
		public async Task<IActionResult> CreateUserTeam()
		{
			int idUser = HttpContext.Session.GetObject<UserPlayer>("Usuario").IdUser;

			int userTeams = await this.repo.FindUserTeams(idUser);

			List<Team> teams = await this.repo.GetTeams();
			ViewData["Teams"] = teams;

			if (userTeams == 0)
			{
				return View();
			}
			else
			{
				return RedirectToAction("Error", "Home");
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateUserTeam(string userTeamName, int idUser, int idTeam)
		{
			await this.repo.CreateUserTeam(userTeamName, idUser, idTeam);

			return RedirectToAction("TeamPage");
		}
		#endregion

		[AuthorizeUsers]
		#region CREATE USER TEAM DRIVERS
		//VISTA PARA ADMINISTRAR EL EQUIPO (CREAR/VISUALIZAR/ACTUALIZAR/ELIMINAR)(CRUD)
		public async Task<IActionResult> CreateUserTeamDrivers()
		{
			// EXTRAIGO LAS CARRERAS
			List<Race> racesNow = await this.repo.GetRacesNow();

			foreach (Race r in racesNow)
			{
				DateTime dateRaceStart = r.GpDateStart;
				DateTime dateRaceEnd = r.GpDateEnd;
				DateTime today = DateTime.Now;

				if (today >= dateRaceStart && today <= dateRaceEnd)
				{
					return RedirectToAction("Index");
				}
			}

			//EXTRAE EL ID DEL USUARIO
			int idUser = HttpContext.Session.GetObject<UserPlayer>("Usuario").IdUser;

			//SACA EL EQUIPO SIN LOS PILOTOS DEL USUARIO
			UserTeam userTeam = await this.repo.GetUserTeam(idUser);

			//SACA TODOS LOS VALORES DEL EQUIPO ENTERO DEL USUARIO
			List<VistaUserTeam> userTeams = await this.repo.GetVistaUserTeams(idUser);
			ViewData["UserTeams"] = userTeams;

			//EXTRAE LAS ESCUDERIAS
			List<Team> teams = await this.repo.GetTeams();
			ViewData["Teams"] = teams;

			//EXTRAE LOS PILOTOS
			List<Driver> drivers = await this.repo.GetDrivers();
			ViewData["Drivers"] = drivers;

			//CONTAREMOS CUANTOS PILOTOS TIENE EL USUARIO PARA QUE NO SUPERE MAS DE 5
			List<DriverUserTeam> driverUserTeam = await this.repo.FindDriverUserTeam(userTeam.IdUserTeam);

			decimal totalPrice = 100 - teams[userTeam.IdTeam - 1].Price;

			foreach (DriverUserTeam d in driverUserTeam)
			{
				foreach (Driver ds in drivers)
				{
					if (d.IdDriver == ds.IdDriver)
					{
						totalPrice = totalPrice - ds.Price;
					}
				}
			}

			ViewData["PilotoExistente"] = "";
			ViewData["TotalPrice"] = totalPrice;
			ViewData["Presupuesto"] = "";

			return View(userTeam);
		}

		[HttpPost]
		public async Task<IActionResult> CreateUserTeamDrivers(int idDriver, int idTeam)
		{
			//EXTRAE EL ID DEL USUARIO
			int idUser = HttpContext.Session.GetObject<UserPlayer>("Usuario").IdUser;

			//EXTRAE EL EQUIPO DEL USUARIO
			UserTeam userTeam = await this.repo.GetUserTeam(idUser);

			//SACA TODOS LOS VALORES DEL EQUIPO ENTERO DEL USUARIO
			List<VistaUserTeam> userTeams = await this.repo.GetVistaUserTeams(idUser);
			ViewData["UserTeams"] = userTeams;

			//EXTRAEMOS LOS PILOTOS
			List<Driver> drivers = await this.repo.GetDrivers();
			ViewData["Drivers"] = drivers;

			//EXTRAE LAS ESCUDERIAS
			List<Team> teams = await this.repo.GetTeams();
			ViewData["Teams"] = teams;

			//CONTAREMOS CUANTOS PILOTOS TIENE EL USUARIO PARA QUE NO SUPERE MAS DE 5
			List<DriverUserTeam> driverUserTeam = await this.repo.FindDriverUserTeam(userTeam.IdUserTeam);

			//FUNCION PARA SABER SI YA ESTA EL PILOTO EN EL EQUIPO
			List<DriverUserTeam> pilotoExistente = await this.repo.FindDriverInTeamPlayer(userTeam.IdUserTeam, idDriver);

			//CALCULO DE PRECIO PARA LA ACTUALIZACION DEL PILOTO
			decimal totalPrice = 100 - teams[userTeam.IdTeam - 1].Price;

			foreach (DriverUserTeam d in driverUserTeam)
			{
				foreach (Driver dr in drivers)
				{
					if (d.IdDriver == dr.IdDriver)
					{
						totalPrice = totalPrice - dr.Price;
					}
				}
			}

			//CALCULO DE PRECIO PARA LA ACTUALIZACION DEL TEAM

			//EXTRAEMOS TODOS LOS DATOS DEL EQUIPO DEL USUARIO
			List<VistaUserTeam> vistaUserTeam = await this.repo.GetVistaUserTeams(idUser);

			decimal totalPriceDrivers = 0;
			decimal totalPriceTeam = 100;

			for (int i = 0; i < vistaUserTeam.Count(); i++)
			{
				totalPriceDrivers += vistaUserTeam[i].DriverPrice;
			}

			foreach (Team t in teams)
			{
				if (t.IdTeam == idTeam)
				{
					//RESTAMOS EL VALOR DEL NUEVO EQUIPO AL TOTAL
					totalPriceTeam -= t.Price;

					//RESTAMOS EL VALOR DE TODOS LOS PILOTOS AL TOTAL
					totalPriceTeam -= totalPriceDrivers;
				}
			}

			if (idDriver != 0)
			{
				if (driverUserTeam.Count() < 5)
				{
					if (pilotoExistente.Count == 0)
					{
						foreach (Driver d in drivers)
						{
							if (d.IdDriver == idDriver)
							{
								totalPrice = totalPrice - d.Price;

								if (totalPrice <= userTeam.TeamMoney && totalPrice > 0)
								{
									//CREAMOS EL REGISTRO DEL PILOTO
									await this.repo.CreateUserTeamDriver(userTeam.IdUserTeam, idDriver);

									return RedirectToAction("CreateUserTeamDrivers", userTeam);
								}

								ViewData["Presupuesto"] = "With that choice you exceed the budget limit";
								totalPrice = totalPrice + d.Price;
								ViewData["TotalPrice"] = totalPrice;
								ViewData["PilotoExistente"] = "";
								return View(userTeam);
							}
						}
					}
					ViewData["PilotoExistente"] = "That driver is already selected";
					ViewData["TotalPrice"] = totalPrice;
					ViewData["Presupuesto"] = "";
					return View(userTeam);
				}
				ViewData["PilotoExistente"] = "Your team is complete";
				ViewData["TotalPrice"] = totalPrice;
				ViewData["Presupuesto"] = "";
				return View(userTeam);
			}
			else if(idTeam != 0)
			{
				if (totalPriceTeam > 0)
				{
					await this.repo.UpdateTeamUserPlayer(idUser, userTeam.IdUserTeam, idTeam);

					return RedirectToAction("CreateUserTeamDrivers", userTeam);
				}

				ViewData["Presupuesto"] = "With that choice you exceed the budget limit";
				ViewData["TotalPrice"] = totalPrice;
				ViewData["PilotoExistente"] = "";
				return View(userTeam);
			}

			return View(userTeam);
		}
		#endregion

		[AuthorizeUsers]
		#region REMOVE USER TEAM DRIVER
		[HttpPost]
		public async Task<IActionResult> RemoveUserTeamDriver(int idDriverClose)
		{
			//EXTRAE EL ID DEL USUARIO
			int idUser = HttpContext.Session.GetObject<UserPlayer>("Usuario").IdUser;

			//SACA EL EQUIPO SIN LOS PILOTOS DEL USUARIO
			UserTeam userTeam = await this.repo.GetUserTeam(idUser);

			await this.repo.RemoveUserTeamDriver(userTeam.IdUserTeam, idDriverClose);

			return RedirectToAction("CreateUserTeamDrivers", userTeam);
		}
		#endregion

		[AuthorizeUsers]
		#region VISTA AÑADIR LOS PILOTOS
		public async Task<IActionResult> InsertDrivers()
		{
			//EXTRAE EL ID DEL USUARIO
			int idUser = HttpContext.Session.GetObject<UserPlayer>("Usuario").IdUser;

			//EXTRAE EL EQUIPO DEL USUARIO
			UserTeam userTeam = await this.repo.GetUserTeam(idUser);

			//EXTRAEMOS ID DEL EQUIPO DEL USUARIO
			int idTeamUser = userTeam.IdUserTeam;

			//EXTRAEMOS LOS PILOTOS
			List<Driver> drivers = await this.repo.GetDrivers();

			//CONTAREMOS CUANTOS PILOTOS TIENE EL USUARIO PARA QUE NO SUPERE MAS DE 5
			List<DriverUserTeam> driverUserTeam = await this.repo.FindDriverUserTeam(idTeamUser);

			ViewData["PilotoExistente"] = "null";

			if (driverUserTeam.Count() <= 5)
			{
				ViewData["Selected"] = "";
				ViewData["Disable"] = driverUserTeam.Count();

				return View(drivers);
			}

			return RedirectToAction("TeamPage");
		}

		[HttpPost]
		public async Task<IActionResult> InsertDrivers(int idDriver)
		{
			//EXTRAE EL ID DEL USUARIO
			int idUser = HttpContext.Session.GetObject<UserPlayer>("Usuario").IdUser;

			//EXTRAE EL EQUIPO DEL USUARIO
			UserTeam userTeam = await this.repo.GetUserTeam(idUser);

			//EXTRAEMOS ID DEL EQUIPO DEL USUARIO
			int idTeamUser = userTeam.IdUserTeam;

			//EXTRAEMOS LOS PILOTOS
			List<Driver> drivers = await this.repo.GetDrivers();

			//CONTAREMOS CUANTOS PILOTOS TIENE EL USUARIO PARA QUE NO SUPERE MAS DE 5
			List<DriverUserTeam> driverUserTeam = await this.repo.FindDriverUserTeam(idTeamUser);

			//FUNCION PARA SABER SI YA ESTA EL PILOTO EN EL EQUIPO
			List<DriverUserTeam> pilotoExistente = await this.repo.FindDriverInTeamPlayer(idTeamUser, idDriver);

			if (driverUserTeam.Count() < 5)
			{
				if (pilotoExistente.Count() != 0)
				{
					ViewData["PilotoExistente"] = "This driver has already been selected";

					ViewData["Selected"] = "You selected " + driverUserTeam.Count() + " drivers.";
					ViewData["Disable"] = driverUserTeam.Count() - 1;
					return View(drivers);
				}

				//CREAMOS EL REGISTRO DEL PILOTO
				await this.repo.CreateUserTeamDriver(idTeamUser, idDriver);

				ViewData["PilotoExistente"] = "null";
				ViewData["Selected"] = "You selected " + (driverUserTeam.Count() + 1) + " drivers.";
				ViewData["Disable"] = driverUserTeam.Count();

				return View(drivers);
			}

			return RedirectToAction("TeamPage");
		}
		#endregion

		[AuthorizeUsers]
		#region VISTA DE LAS LIGAS
		public async Task<IActionResult> Leagues(int? posicion)
		{
			//SI LA POSICION ES NULL DEVUELVE 0, SI NO EL VALOR DE POSICION
			posicion = (posicion == null) ? 0 : posicion.Value;

			PaginacionLeagues itemsPaginados = await this.repo.PaginacionLeaguesAsync(posicion.Value);
            ViewData["Leagues"] = itemsPaginados.Leagues;
            ViewData["REGISTROS"] = itemsPaginados.NumRegistros;

			//EXTRAE EL ID DEL USUARIO
			int idUser = HttpContext.Session.GetObject<UserPlayer>("Usuario").IdUser;

			List<VistaLeague> vistaLeague = await this.repo.FindVistaLeague(idUser);

			return View(vistaLeague);
		}
		#endregion

		[AuthorizeUsers]
		#region VISTA DE UNA LIGA
		public async Task<IActionResult> League(int idLeague)
		{
			//EXTRAE EL ID DEL USUARIO
			int idUser = HttpContext.Session.GetObject<UserPlayer>("Usuario").IdUser;

			List<VistaLeague> vistaLeague = await this.repo.FindVistaLeagueMembers(idLeague);

			List<VistaLeague> userInLeague = await this.repo.FindUserInLeague(idLeague, idUser);
			ViewData["UserInLeague"] = userInLeague;

			ViewData["League"] = idLeague;

			League league = await this.repo.FindLeague(idLeague);
			ViewData["LeagueUnicque"] = league;

			return View(vistaLeague);
		}
		#endregion

		[AuthorizeUsers]
		#region VISTA/ACCION JOIN LEAGUE
		[HttpPost]
		public async Task<IActionResult> JoinLeague(int idLeague)
		{
			//EXTRAE EL ID DEL USUARIO
			int idUser = HttpContext.Session.GetObject<UserPlayer>("Usuario").IdUser;

			await this.repo.InsertUserLeague(idUser, idLeague);

			return RedirectToAction("Leagues");
		}
		#endregion

		[AuthorizeUsers]
		#region VISTA/ACCION CREATE LEAGUE
		[HttpPost]
		public async Task<IActionResult> CreateLeague(string leagueName)
		{
			//EXTRAE EL ID DEL USUARIO
			int idUser = HttpContext.Session.GetObject<UserPlayer>("Usuario").IdUser;

			await this.repo.CreateLeague(idUser, leagueName);

			return RedirectToAction("Leagues");
		}
		#endregion

		[AuthorizeUsers]
		#region VISTA/ACCION DELETE USER IN THIS LEAGUE
		[HttpPost]
		public async Task<IActionResult> RemoveUserLeague(int idLeague)
		{
			//EXTRAE EL ID DEL USUARIO
			int idUser = HttpContext.Session.GetObject<UserPlayer>("Usuario").IdUser;

			await this.repo.RemoveUserLeague(idUser, idLeague);

			return RedirectToAction("Leagues");
		}
		#endregion

		[AuthorizeUsers]
		#region VISTA BUSCADOR LEAGUE CODE
		public async Task<IActionResult> FindLeagueCode(int leagueCode)
		{
			League league = await this.repo.FindLeagueCode(leagueCode);

			if (league != null)
			{
				return RedirectToAction("League", new { idLeague = league.IdLeague });
			}

			return RedirectToAction("Leagues");
		}
		#endregion

		[Authorize(Roles = "Admin")]
		#region VISTA/ACCION AÑADIR PUNTUACIONES A PILOTOS
		public async Task<IActionResult> PointsDriver()
		{
			List<Race> races = await this.repo.GetRaces();
			return View(races);
		}

		[HttpPost]
		public async Task<IActionResult> PointsDriver(int idRace)
		{
			await this.repo.InsertPointsDriver(idRace);

			return RedirectToAction("AdminPage");
		}
		#endregion

		[Authorize(Roles = "Admin")]
		#region VISTA/ACCION ELIMINAR PUNTUACIONES A PILOTOS
		public async Task<IActionResult> PointsDriverRemove()
		{
			List<Race> races = await this.repo.GetRaces();
			return View(races);
		}

		[HttpPost]
		public async Task<IActionResult> PointsDriverRemove(int idRace)
		{
			await this.repo.RemovePointsDriver(idRace);

			return RedirectToAction("AdminPage");
		}
		#endregion

		[Authorize(Roles = "Admin")]
		#region VISTA/ACCION RESTABLECER PUNTUACIONES A 0 DE LOS PILOTOS
		[HttpPost]
		public async Task<IActionResult> RestartPointsDriver()
		{
			await this.repo.RestartPointsDriver();

			return RedirectToAction("AdminPage");
		}
		#endregion

		[Authorize(Roles = "Admin")]
		#region VISTA/ACCION AÑADIR PUNTUACION A LOS USUARIOS DE LAS LIGAS
		public async Task<IActionResult> PointsUserLeagues()
		{
			List<Race> races = await this.repo.GetRaces();
			return View(races);
		}

		[HttpPost]
		public async Task<IActionResult> PointsUserLeagues(int idRace)
		{
			List<VistaLeague> userPlayers = await this.repo.GetVistaLeague();

			foreach (VistaLeague u in userPlayers)
			{
				UserTeam userTeam = await this.repo.GetUserTeam(u.IdUSer);

				await this.repo.InsertPointsUserClassification(u.IdUSer, u.IdLeague, idRace, userTeam.IdUserTeam);
			}

			return RedirectToAction("AdminPage");
		}
		#endregion
	}
}
