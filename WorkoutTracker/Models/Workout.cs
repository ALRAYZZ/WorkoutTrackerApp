namespace WorkoutTracker.Models
{
	public class Workout
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public TimeSpan Duration { get; set; }
		public List<Exercise> Exercises { get; set; }
		public DateTime StartDate { get; set; }
	}
}
