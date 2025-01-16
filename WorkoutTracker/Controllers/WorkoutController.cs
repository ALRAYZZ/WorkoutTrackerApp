using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using WorkoutTracker.Models;
using WorkoutTracker.Services;

namespace WorkoutTracker.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Authorize]
	public class WorkoutController : Controller
	{
		private readonly WorkoutService _workoutService;

		public WorkoutController(WorkoutService workoutService)
		{
			_workoutService=workoutService;
		}
		/// <summary>
		/// Create a new workout
		/// </summary>
		/// <param name="workoutDto">The Workout details.</param>
		/// <returns>The created workout.</returns>
		[HttpPost]
		public async Task<ActionResult<Workout>> CreateWorkout(WorkoutCreateDto workoutDto)
		{
			var userId = int.Parse(User.Claims.First(c => c.Type == "userId").Value);

			var workout = new Workout()
			{
				UserId = workoutDto.UserId,
				Duration = workoutDto.Duration,
				StartDate = workoutDto.StartDate,
				Exercises = workoutDto.Exercises.Select(e => new Exercise()
				{
					Type = e.Type,
					Description = e.Description,
					Sets = e.Sets,
					Repetitions = e.Repetitions,
					Weight = e.Weight
				}).ToList()

			};
			var createdWorkout = await _workoutService.CreateWorkoutAsync(workout);
			return Ok(createdWorkout);
		}
		/// <summary>
		/// Get all workouts for the authenticated user
		/// </summary>
		/// <returns>A list of workouts.</returns>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Workout>>> GetWorkouts()
		{
			var authenticatedUserId = int.Parse(User.Claims.First(c => c.Type == "userId").Value);
			

			var workouts = await _workoutService.GetWorkoutsByUserIdAsync(authenticatedUserId);
			if (workouts == null)
			{
				return NotFound();
			}
			return Ok(workouts);
		}


		/// <summary>
		/// Updated an existing workout.
		/// </summary>
		/// <param name="id">The workout ID.</param>
		/// <param name="workoutDto">The updated workout details.</param>
		/// <returns>The updated workout.</returns>
		[HttpPut("{id}")]
		public async Task<ActionResult<Workout>> UpdateWorkout(int id, WorkoutCreateDto workoutDto)
		{
			var workout = await _workoutService.GetWorkoutByIdAsync(id);
			if (workout == null)
			{
				return NotFound();
			}

			var userId = int.Parse(User.Claims.First(c => c.Type == "userId").Value);
			if (workout.UserId != userId)
			{
				return Forbid();
			}

			workout.Duration = workoutDto.Duration;
			workout.StartDate = workoutDto.StartDate;
			workout.Exercises = workoutDto.Exercises.Select(e => new Exercise()
			{
				Type = e.Type,
				Description = e.Description,
				Sets = e.Sets,
				Repetitions = e.Repetitions,
				Weight = e.Weight
			}).ToList();

			var updatedWorkout = await _workoutService.UpdateWorkoutAsync(workout);
			return Ok(updatedWorkout);
		}
		/// <summary>
		/// Deletes a workout.
		/// </summary>
		/// <param name="id">The workout ID.</param>
		/// <returns>No content.</returns>
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteWorkout(int id)
		{
			var workout = await _workoutService.GetWorkoutByIdAsync(id);
			if (workout == null)
			{
				return NotFound();
			}

			var userId = int.Parse(User.Claims.First(c => c.Type == "userId").Value);
			if (workout.UserId != userId)
			{
				return Forbid();
			}
			
			var result = await _workoutService.DeleteWorkoutAsync(id);
			if (result)
			{
				return Ok();
			}
			else
			{
				return NotFound();
			}
		}
		/// <summary>
		/// Gets pending workouts for the authenticated user.
		/// </summary>
		/// <returns>A list of pending workouts.</returns>
		[HttpGet("pending")]
		public async Task<ActionResult<IEnumerable<Workout>>> GetPendingWorkoutsByUserId()
		{
			var authenticatedUserId = int.Parse(User.Claims.First(c => c.Type == "userId").Value);

			var workouts = await _workoutService.GetPendingWorkoutsByUserIdAsync(authenticatedUserId);
			if (workouts == null)
			{
				return NotFound("No pending workouts found.");
			}
			return Ok(workouts);
		}
		/// <summary>
		/// Gets completed workouts for the authenticated user.
		/// </summary>
		/// <returns>A list of completed workouts.</returns>
		[HttpGet("completed")]
		public async Task<ActionResult<IEnumerable<Workout>>> GetCompletedWorkoutsByUserId()
		{
			var authenticatedUserId = int.Parse(User.Claims.First(c => c.Type == "userId").Value);
			
			var workouts = await _workoutService.GetCompletedWorkoutsByUserIdAsync(authenticatedUserId);
			if (workouts == null)
			{
				return NotFound("No completed workouts found.");
			}
			return Ok(workouts);
		}
	}
}
