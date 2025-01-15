using Microsoft.EntityFrameworkCore;
using WorkoutTracker.DataAccess;
using WorkoutTracker.Models;

namespace WorkoutTracker.Services
{
	public class WorkoutService
	{
		private readonly WorkoutTrackerDbContext _context;

		public WorkoutService(WorkoutTrackerDbContext context)
		{
			_context=context;
		}


		public async Task<Workout> CreateWorkoutAsync(Workout workout)
		{
			_context.Workouts.Add(workout);
			await _context.SaveChangesAsync();
			return workout;
		}

		public async Task<List<Workout>> GetWorkoutsByUserIdAsync(int userId)
		{
			//We need to Include because EF Core will not automatically load related data
			return await _context.Workouts.Include(w => w.Exercises).Where(w => w.Id == userId).ToListAsync();
		}
		public async Task<Workout> GetWorkoutByIdAsync(int id)
		{
			return await _context.Workouts.Include(w => w.Exercises).FirstOrDefaultAsync(w => w.Id == id);
		}

		public async Task<List<Workout>> GetPendingWorkoutsByUserIdAsync(int userId)
		{
			return await _context.Workouts.Include(w =>w.Exercises)
				.Where(w => w.UserId == userId && w.StartDate > DateTime.Now)
				.ToListAsync();
		}

		public async Task<List<Workout>> GetCompletedWorkoutsByUserIdAsync(int userId)
		{
			return await _context.Workouts.Include(w => w.Exercises)
				.Where(w => w.UserId == userId && w.StartDate < DateTime.Now)
				.ToListAsync();
		}


		public async Task<Workout> UpdateWorkoutAsync(Workout workout)
		{
			_context.Workouts.Update(workout);
			await _context.SaveChangesAsync();
			return workout;
		}

		public async Task<bool> DeleteWorkoutAsync(int id)
		{
			var workout = await _context.Workouts.FindAsync(id);
			if (workout == null)
			{
				return false;
			}

			_context.Workouts.Remove(workout);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
