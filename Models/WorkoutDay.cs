namespace FitnessPlanner.Models;

public sealed class WorkoutDay
{
    public int Id { get; set; }
    public int PlanId { get; set; }
    public required WorkoutPlan Plan { get; set; }
    public int DayNumber { get; set; }
    public required string Title { get; set; }
    public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
}
