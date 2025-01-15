using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutTracker.Models;
using WorkoutTracker.Services;

namespace WorkoutTracker.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : Controller
	{
		private readonly UserService _userService;
		
		public UsersController(UserService userService)
		{
			_userService = userService;
		}

		[HttpPost("register")]
		[AllowAnonymous]
		public async Task<ActionResult<RegisterUser>> RegisterUser(RegisterUser registerModel)
		{
			var userModel = new UserModel()
			{
				Name = registerModel.Name,
				Username = registerModel.Username,
				Password = registerModel.Password,
				Email = registerModel.Email
			};
			var createdUser = await _userService.RegisterUserAsync(userModel);
			return CreatedAtAction(nameof(RegisterUser), new { id = createdUser.Id }, createdUser);
		}

		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> LoginUser(LoginModel login)
		{
			var user = await _userService.LoginUserAsync(login);
			if (user == null)
			{
				return Unauthorized();
			}
			var token = _userService.GenerateJwtToken(user);
			return Ok(new {Token = token });
		}
	}
}
