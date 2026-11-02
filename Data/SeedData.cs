using FitnessPlanner.Models;

namespace FitnessPlanner.Data;

public static class SeedData
{
    private static readonly Exercise[] Exercises =
    {
        new() { Name = "Приседания", MuscleGroup = MuscleGroup.Legs, Difficulty = FitnessLevel.Beginner, RequiredEquipment = "Без оборудования" },
        new() { Name = "Выпады", MuscleGroup = MuscleGroup.Legs, Difficulty = FitnessLevel.Beginner, RequiredEquipment = "Без оборудования" },
        new() { Name = "Ягодичный мост", MuscleGroup = MuscleGroup.Legs, Difficulty = FitnessLevel.Beginner, RequiredEquipment = "Без оборудования" },
        new() { Name = "Подъёмы на носки", MuscleGroup = MuscleGroup.Legs, Difficulty = FitnessLevel.Beginner, RequiredEquipment = "Без оборудования" },
        new() { Name = "Отжимания", MuscleGroup = MuscleGroup.Chest, Difficulty = FitnessLevel.Intermediate, RequiredEquipment = "Без оборудования" },
        new() { Name = "Отжимания с колен", MuscleGroup = MuscleGroup.Chest, Difficulty = FitnessLevel.Beginner, RequiredEquipment = "Без оборудования" },
        new() { Name = "Жим гантелей лёжа", MuscleGroup = MuscleGroup.Chest, Difficulty = FitnessLevel.Intermediate, RequiredEquipment = "Гантели" },
        new() { Name = "Супермен", MuscleGroup = MuscleGroup.Back, Difficulty = FitnessLevel.Beginner, RequiredEquipment = "Без оборудования" },
        new() { Name = "Тяга гантели в наклоне", MuscleGroup = MuscleGroup.Back, Difficulty = FitnessLevel.Beginner, RequiredEquipment = "Гантели" },
        new() { Name = "Тяга эспандера", MuscleGroup = MuscleGroup.Back, Difficulty = FitnessLevel.Beginner, RequiredEquipment = "Эспандер" },
        new() { Name = "Планка", MuscleGroup = MuscleGroup.Core, Difficulty = FitnessLevel.Beginner, RequiredEquipment = "Без оборудования" },
        new() { Name = "Скручивания", MuscleGroup = MuscleGroup.Core, Difficulty = FitnessLevel.Beginner, RequiredEquipment = "Без оборудования" },
        new() { Name = "Жим гантелей над головой", MuscleGroup = MuscleGroup.Shoulders, Difficulty = FitnessLevel.Intermediate, RequiredEquipment = "Гантели" },
        new() { Name = "Отжимания уголком", MuscleGroup = MuscleGroup.Shoulders, Difficulty = FitnessLevel.Advanced, RequiredEquipment = "Без оборудования" },
        new() { Name = "Сгибание рук с гантелями", MuscleGroup = MuscleGroup.Biceps, Difficulty = FitnessLevel.Beginner, RequiredEquipment = "Гантели" },
        new() { Name = "Разгибание рук с эспандером", MuscleGroup = MuscleGroup.Triceps, Difficulty = FitnessLevel.Beginner, RequiredEquipment = "Эспандер" },
        new() { Name = "Отжимания узким хватом", MuscleGroup = MuscleGroup.Triceps, Difficulty = FitnessLevel.Intermediate, RequiredEquipment = "Без оборудования" }
    };

    public static void EnsureSeeded(FitnessDbContext db)
    {
        if (db.Exercises.Any())
        {
            return;
        }

        db.Exercises.AddRange(Exercises);
        db.SaveChanges();
    }
}
