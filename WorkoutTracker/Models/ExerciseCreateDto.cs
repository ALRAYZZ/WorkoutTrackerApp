namespace WorkoutTracker.Models
{
	public class ExerciseCreateDto
	{
		public string Type { get; set; }
		public string Description { get; set; }
		public int Sets { get; set; }
		public int Repetitions { get; set; }
		public double Weight { get; set; }
	}
}
