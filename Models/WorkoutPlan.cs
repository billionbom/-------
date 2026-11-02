namespace FitnessPlanner.Models;

public sealed class WorkoutPlan
{
    public int Id { get; set; }
    public int ProfileId { get; set; }
    public required FitnessProfile Profile { get; set; }
    public FitnessGoal Goal { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public int DurationWeeks { get; set; } = 4;
    public ICollection<WorkoutDay> WorkoutDays { get; set; } = new List<WorkoutDay>();
}
