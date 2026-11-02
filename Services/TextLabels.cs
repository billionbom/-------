using FitnessPlanner.Models;

namespace FitnessPlanner.Services;

public static class TextLabels
{
    public static string Goal(FitnessGoal goal) => goal switch
    {
        FitnessGoal.Strength => "развитие силы",
        FitnessGoal.Endurance => "выносливость",
        FitnessGoal.WeightManagement => "контроль веса",
        FitnessGoal.GeneralFitness => "общая физическая форма",
        _ => throw new ArgumentOutOfRangeException(nameof(goal), goal, "Неизвестная цель тренировок.")
    };

    public static string Level(FitnessLevel level) => level switch
    {
        FitnessLevel.Beginner => "начальный",
        FitnessLevel.Intermediate => "средний",
        FitnessLevel.Advanced => "продвинутый",
        _ => throw new ArgumentOutOfRangeException(nameof(level), level, "Неизвестный уровень подготовки.")
    };
}
