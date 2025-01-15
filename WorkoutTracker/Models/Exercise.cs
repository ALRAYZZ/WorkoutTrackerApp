namespace WorkoutTracker.Models
{
	public class Exercise
	{
		public int Id { get; set; }
		public string Type { get; set; }
		public string Description { get; set; }
		public int Sets { get; set; }
		public int Repetitions { get; set; }
		public double Weight { get; set; }
	}
}
