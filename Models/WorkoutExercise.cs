namespace FitnessPlanner.Models;

public sealed class WorkoutExercise
{
    public int Id { get; set; }
    public int WorkoutDayId { get; set; }
    public required WorkoutDay WorkoutDay { get; set; }
    public int ExerciseId { get; set; }
    public required Exercise Exercise { get; set; }
    public int Order { get; set; }
    public int Sets { get; set; }
    public required string Repetitions { get; set; }
    public int RestSeconds { get; set; }
}
