using WorkoutTracker.Models;

namespace WorkoutTracker.DataAccess
{
	public class DataSeeder
	{
		private readonly WorkoutTrackerDbContext _context;

		public DataSeeder(WorkoutTrackerDbContext context)
		{
			_context=context;
		}
		

		public void Seed()
		{
			if (!_context.Exercises.Any())
			{
				var exercises = new List<Exercise>()
				{
					new Exercise {Type = "Bench Press", Description = "Lay flat on a bench and push the barbell up", Sets = 3, Repetitions = 10, Weight = 135 },
					new Exercise {Type = "Squats", Description = "Stand with the barbell on your shoulders and squat down", Sets = 3, Repetitions = 10, Weight = 225 },
					new Exercise {Type = "Deadlifts", Description = "Stand with the barbell on the ground and lift it up", Sets = 3, Repetitions = 10, Weight = 315 }
				};

				_context.Exercises.AddRange(exercises);
				_context.SaveChanges();
			}
		}
	}
}
