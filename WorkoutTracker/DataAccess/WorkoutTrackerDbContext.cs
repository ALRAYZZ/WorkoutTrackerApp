using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Models;

namespace WorkoutTracker.DataAccess
{
	public class WorkoutTrackerDbContext : DbContext
	{
		public WorkoutTrackerDbContext(DbContextOptions<WorkoutTrackerDbContext> options) : base(options)
		{
		}

		public DbSet<Workout> Workouts { get; set; }
		public DbSet<Exercise> Exercises { get; set; }
		public DbSet<UserModel> Users { get; set; }
	}
}
