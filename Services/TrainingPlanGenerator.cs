using FitnessPlanner.Models;

namespace FitnessPlanner.Services;

public sealed class TrainingPlanGenerator
{
    private static readonly MuscleGroup[] FullBody =
    {
        MuscleGroup.Legs, MuscleGroup.Chest, MuscleGroup.Back, MuscleGroup.Core
    };

    private static readonly MuscleGroup[] UpperBody =
    {
        MuscleGroup.Chest, MuscleGroup.Back, MuscleGroup.Shoulders, MuscleGroup.Biceps, MuscleGroup.Triceps
    };

    private static readonly MuscleGroup[] LowerBody =
    {
        MuscleGroup.Legs, MuscleGroup.Core
    };

    private static readonly MuscleGroup[] Push =
    {
        MuscleGroup.Chest, MuscleGroup.Shoulders, MuscleGroup.Triceps
    };

    private static readonly MuscleGroup[] Pull =
    {
        MuscleGroup.Back, MuscleGroup.Biceps
    };

    private static readonly MuscleGroup[] Legs =
    {
        MuscleGroup.Legs, MuscleGroup.Core
    };

    public WorkoutPlan Generate(FitnessProfile profile, IReadOnlyCollection<Exercise> catalog)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(catalog);

        var plan = new WorkoutPlan
        {
            Profile = profile,
            ProfileId = profile.Id,
            Goal = profile.Goal,
            DurationWeeks = 4
        };
        var availableExercises = catalog
            .Where(exercise => exercise.Difficulty <= profile.Level)
            .Where(exercise => IsEquipmentAvailable(exercise.RequiredEquipment, profile.Equipment))
            .ToList();

        for (var index = 0; index < profile.TrainingDaysPerWeek; index++)
        {
            var groups = GroupsForDay(profile.TrainingDaysPerWeek, index);
            var day = new WorkoutDay
            {
                Plan = plan,
                DayNumber = index + 1,
                Title = DayTitle(profile.TrainingDaysPerWeek, index)
            };

            var order = 1;
            foreach (var group in groups)
            {
                var candidates = availableExercises
                    .Where(exercise => exercise.MuscleGroup == group)
                    .OrderBy(exercise => exercise.Id)
                    .ThenBy(exercise => exercise.Name)
                    .ToList();
                if (candidates.Count == 0)
                {
                    continue;
                }

                var exercise = candidates[index % candidates.Count];
                var (sets, repetitions, restSeconds) = Prescription(profile);
                day.WorkoutExercises.Add(new WorkoutExercise
                {
                    WorkoutDay = day,
                    Exercise = exercise,
                    ExerciseId = exercise.Id,
                    Order = order++,
                    Sets = sets,
                    Repetitions = repetitions,
                    RestSeconds = restSeconds
                });
            }

            if (day.WorkoutExercises.Count == 0)
            {
                throw new InvalidOperationException("Для профиля не удалось подобрать упражнения. Проверьте каталог и доступный инвентарь.");
            }

            plan.WorkoutDays.Add(day);
        }

        return plan;
    }

    private static bool IsEquipmentAvailable(string requiredEquipment, string availableEquipment)
    {
        if (requiredEquipment == "Без оборудования")
        {
            return true;
        }

        return availableEquipment
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Contains(requiredEquipment, StringComparer.CurrentCultureIgnoreCase);
    }

    private static MuscleGroup[] GroupsForDay(int daysPerWeek, int dayIndex)
    {
        if (daysPerWeek <= 3)
        {
            return FullBody;
        }

        if (daysPerWeek == 4)
        {
            return dayIndex % 2 == 0 ? UpperBody : LowerBody;
        }

        return (dayIndex % 3) switch
        {
            0 => Push,
            1 => Pull,
            _ => Legs
        };
    }

    private static string DayTitle(int daysPerWeek, int dayIndex)
    {
        if (daysPerWeek <= 3)
        {
            return "Тренировка всего тела";
        }

        if (daysPerWeek == 4)
        {
            return dayIndex % 2 == 0 ? "Верхняя часть тела" : "Нижняя часть тела и корпус";
        }

        return (dayIndex % 3) switch
        {
            0 => "Толкающие мышцы",
            1 => "Тянущие мышцы",
            _ => "Ноги и корпус"
        };
    }

    private static (int Sets, string Repetitions, int RestSeconds) Prescription(FitnessProfile profile)
    {
        var durationSetLimit = profile.SessionDurationMinutes <= 30
            ? 2
            : profile.SessionDurationMinutes >= 75 ? 4 : 3;

        if (profile.Goal == FitnessGoal.Strength)
        {
            var strengthSets = Math.Min(profile.Level == FitnessLevel.Beginner ? 3 : 4, durationSetLimit);
            return (strengthSets, "6-8", 120);
        }

        return profile.Goal switch
        {
            FitnessGoal.Endurance => (Math.Min(3, durationSetLimit), "12-15", 45),
            FitnessGoal.WeightManagement => (Math.Min(3, durationSetLimit), "10-15", 60),
            _ => (Math.Min(profile.Level == FitnessLevel.Beginner ? 2 : 3, durationSetLimit), "8-12", 75)
        };
    }
}
