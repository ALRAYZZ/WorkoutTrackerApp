using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkoutTracker.DataAccess;
using WorkoutTracker.Models;

namespace WorkoutTracker.Services
{
	public class UserService
	{
		private readonly WorkoutTrackerDbContext _context;
		private readonly IConfiguration _config;

		public UserService(WorkoutTrackerDbContext context, IConfiguration config)
		{
			_context=context;
			_config=config;
		}


		public async Task<UserModel> RegisterUserAsync(UserModel user)
		{
			user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			return user;
		}

		public async Task<UserModel> LoginUserAsync(LoginModel login)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == login.Username);
			if (user == null || !BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
			{
				return null;
			}
			return user;
		}

		public string GenerateJwtToken(UserModel model)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, model.Username),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var token = new JwtSecurityToken(
				issuer: _config["Jwt:Issuer"],
				audience: _config["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(120),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

	}
}
