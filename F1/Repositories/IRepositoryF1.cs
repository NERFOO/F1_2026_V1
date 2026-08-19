using F1.Helpers;
using F1.Models;

namespace F1.Repositories
{
    public interface IRepositoryF1
    {
        Task<UserPlayer> FindUsuario(string email);
        Task RegisterUser(string nickname, string email, string password);
        Task<UserPlayer> LogIn(string email, string password);
        Task UpdateNickname(string nickname, string email);
        Task UpdateEmail(string emailSession, string email);
        Task UpdatePassword(string passSession, string email);
        Task<List<Driver>> GetDrivers();
        Task<List<Team>> GetTeams();
        Task<List<Race>> GetRaces();
        Task<List<Race>> GetRacesNow();
        Task<List<Schedule>> GetSchedule();
        Task<List<VistaUserTeam>> GetVistaUserTeams(int idUser);
        Task<UserTeam> GetUserTeam(int idUser);
        Task<List<League>> GetLeagues();
        Task<List<VistaLeague>> GetVistaLeague();
        Task<List<UserClassification>> GetUserClassification(int idUSer);
        Task<List<VistaResultRace>> FindResultsRace(int idRace);
        Task<Race> FindRace(int idRace);
        Task<int> FindUserTeams(int idUser);
        Task<List<DriverUserTeam>> FindDriverUserTeam(int idUSerTeam);
        Task<List<VistaLeague>> FindVistaLeague(int idUser);
        Task<List<VistaLeague>> FindVistaLeagueMembers(int idLeague);
        Task<List<VistaLeague>> FindVistaLeagueName(string leagueName);
        Task CreateResultRace(int position, int points, string lapTime, int idRace, int idDriver);
        Task CreateUserTeam(string userTeamName, int idUser, int idTeam);
        Task CreateUserTeamDriver(int idTeamUser, int idDriver);
        Task CreateUserTeam(int idTeamUser, int idTeam);
        Task InsertUserLeague(int idUser, int idLeague);
        Task CreateLeague(int idUser, string leagueName);
        Task RemoveUserTeamDriver(int idTeamUser, int idDriver);
        Task RemoveUserTeam(int idTeamUser);
        Task RemoveUserLeague(int idUser, int idLeague);
        Task<List<VistaLeague>> FindUserInLeague(int idLeague, int idUser);
        Task<List<DriverUserTeam>> FindDriverInTeamPlayer(int idUserTeam, int idDriver);
        Task<League> FindLeagueCode(int leagueCode);
        Task<League> FindLeague(int idLeague);
        Task<ResultRace> FindResultRaceDriver(int idRace, int idDriver);
        Task InsertPointsDriver(int idRace);
        Task RemovePointsDriver(int idRace);
        Task RestartPointsDriver();
        Task InsertPointsUserClassification(int idUser, int idLeague, int idRace, int idUserTeam);
        Task<VistaUserClassification> FindUserClassification(int idUser, int idLeague);
        Task<DriverUserTeam> FindDriverUserTeam(int idUSerTeam, int idUser);
        Task UpdateTeamUserPlayer(int idUser, int idTeamUser, int idTeam);
        Task UpdateResultRace(int position, int points, string lapTime, int idRace, int idDriver);
        Task RemoveResultRace(int idRace, int idDriver);
        Task<PaginacionLeagues> PaginacionLeaguesAsync(int posicion);
	}
}
