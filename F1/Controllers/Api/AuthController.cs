using F1.Models;
using F1.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace F1.Controllers.Api
{
	[ApiController]
	[Route("api/auth")]
	public class AuthController : ControllerBase
	{
		private IRepositoryF1 repo;

		public AuthController(IRepositoryF1 repo)
		{
			this.repo = repo;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
			{
				return BadRequest("Email and password required");
			}

			UserPlayer user = await this.repo.LogIn(request.Email, request.Password);

			if (user == null)
			{
				return Unauthorized("Invalid credentials");
			}

			return Ok(new
			{
				user.IdUser,
				user.Nickname,
				user.Email
			});
		}
	}
}
