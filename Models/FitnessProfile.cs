namespace FitnessPlanner.Models;

public sealed class FitnessProfile
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public FitnessGoal Goal { get; set; }
    public FitnessLevel Level { get; set; }
    public int TrainingDaysPerWeek { get; set; }
    public int SessionDurationMinutes { get; set; }
    public string Equipment { get; set; } = string.Empty;
    public ICollection<WorkoutPlan> Plans { get; set; } = new List<WorkoutPlan>();
}
