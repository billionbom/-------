namespace FitnessPlanner.Models;

public sealed class Exercise
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
    public FitnessLevel Difficulty { get; set; }
    public required string RequiredEquipment { get; set; }
}
