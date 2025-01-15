namespace WorkoutTracker.Models
{
	public class WorkoutCreateDto
	{
		public int UserId { get; set; }
		public TimeSpan Duration { get; set; }
		public List<ExerciseCreateDto> Exercises { get; set; }
		public DateTime StartDate { get; set; }
	}
}
