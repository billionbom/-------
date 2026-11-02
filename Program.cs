using FitnessPlanner.Data;
using FitnessPlanner.Models;
using FitnessPlanner.Services;
using Microsoft.EntityFrameworkCore;

var options = new DbContextOptionsBuilder<FitnessDbContext>()
    .UseSqlite("Data Source=fitness.db")
    .Options;

using var db = new FitnessDbContext(options);
db.Database.EnsureCreated();
SeedData.EnsureSeeded(db);

var planGenerator = new TrainingPlanGenerator();
while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== Фитнес-платформа: индивидуальный план тренировок ===");
    Console.WriteLine("1. Создать профиль");
    Console.WriteLine("2. Список профилей");
    Console.WriteLine("3. Составить план тренировок");
    Console.WriteLine("4. Просмотреть сохранённые планы");
    Console.WriteLine("0. Выход");
    var choice = ConsoleInput.ReadInt("Выберите действие: ", 0, 4);

    switch (choice)
    {
        case 1:
            CreateProfile(db);
            break;
        case 2:
            ShowProfiles(db);
            break;
        case 3:
            GeneratePlan(db, planGenerator);
            break;
        case 4:
            ShowPlans(db);
            break;
        case 0:
            return;
    }
}

static void CreateProfile(FitnessDbContext db)
{
    Console.WriteLine("\n--- Новый профиль ---");
    var profile = new FitnessProfile
    {
        Name = ConsoleInput.ReadRequired("Имя: "),
        Goal = ConsoleInput.ReadOption("Цель тренировок:", new[]
        {
            (FitnessGoal.Strength, "Развитие силы"),
            (FitnessGoal.Endurance, "Выносливость"),
            (FitnessGoal.WeightManagement, "Контроль веса"),
            (FitnessGoal.GeneralFitness, "Общая физическая форма")
        }),
        Level = ConsoleInput.ReadOption("Уровень подготовки:", new[]
        {
            (FitnessLevel.Beginner, "Начальный"),
            (FitnessLevel.Intermediate, "Средний"),
            (FitnessLevel.Advanced, "Продвинутый")
        }),
        TrainingDaysPerWeek = ConsoleInput.ReadInt("Тренировок в неделю (2-6): ", 2, 6),
        SessionDurationMinutes = ConsoleInput.ReadInt("Длительность занятия в минутах (20-120): ", 20, 120),
        Equipment = ConsoleInput.ReadOptional("Доступный инвентарь через запятую (например: гантели, коврик; пусто — без оборудования): ")
    };

    db.FitnessProfiles.Add(profile);
    db.SaveChanges();
    Console.WriteLine($"Профиль «{profile.Name}» сохранён (ID: {profile.Id}).");
}

static void ShowProfiles(FitnessDbContext db)
{
    var profiles = db.FitnessProfiles.OrderBy(profile => profile.Name).ToList();
    if (profiles.Count == 0)
    {
        Console.WriteLine("\nПрофилей пока нет. Сначала создайте профиль.");
        return;
    }

    Console.WriteLine("\n--- Профили ---");
    foreach (var profile in profiles)
    {
        Console.WriteLine(
            $"ID {profile.Id}: {profile.Name}; цель — {TextLabels.Goal(profile.Goal)}; " +
            $"уровень — {TextLabels.Level(profile.Level)}; занятий в неделю — {profile.TrainingDaysPerWeek}.");
    }
}

static void GeneratePlan(FitnessDbContext db, TrainingPlanGenerator generator)
{
    var profiles = db.FitnessProfiles.OrderBy(profile => profile.Id).ToList();
    if (profiles.Count == 0)
    {
        Console.WriteLine("\nСначала создайте профиль.");
        return;
    }

    Console.WriteLine("\n--- Выберите профиль ---");
    foreach (var profile in profiles)
    {
        Console.WriteLine($"{profile.Id}. {profile.Name} ({TextLabels.Goal(profile.Goal)})");
    }

    var profileId = ConsoleInput.ReadInt("ID профиля: ", 1, int.MaxValue);
    var selectedProfile = profiles.SingleOrDefault(profile => profile.Id == profileId);
    if (selectedProfile is null)
    {
        Console.WriteLine("Профиль с таким ID не найден.");
        return;
    }

    var catalog = db.Exercises.ToList();
    var plan = generator.Generate(selectedProfile, catalog);
    db.WorkoutPlans.Add(plan);
    db.SaveChanges();
    Console.WriteLine($"\nПлан на {plan.DurationWeeks} недели сохранён.");
    PrintPlan(plan);
}

static void ShowPlans(FitnessDbContext db)
{
    var plans = db.WorkoutPlans
        .Include(plan => plan.Profile)
        .Include(plan => plan.WorkoutDays)
            .ThenInclude(day => day.WorkoutExercises)
                .ThenInclude(item => item.Exercise)
        .OrderByDescending(plan => plan.CreatedAtUtc)
        .ToList();

    if (plans.Count == 0)
    {
        Console.WriteLine("\nСохранённых планов пока нет.");
        return;
    }

    foreach (var plan in plans)
    {
        Console.WriteLine($"\nПлан #{plan.Id} для {plan.Profile.Name}; создан {plan.CreatedAtUtc.ToLocalTime():dd.MM.yyyy HH:mm}; цель — {TextLabels.Goal(plan.Goal)}.");
        PrintPlan(plan);
    }
}

static void PrintPlan(WorkoutPlan plan)
{
    foreach (var day in plan.WorkoutDays.OrderBy(day => day.DayNumber))
    {
        Console.WriteLine($"\nДень {day.DayNumber}: {day.Title}");
        foreach (var item in day.WorkoutExercises.OrderBy(item => item.Order))
        {
            Console.WriteLine($"  {item.Order}. {item.Exercise.Name} — {item.Sets} подхода(ов) × {item.Repetitions}; отдых {item.RestSeconds} сек.");
        }
    }

    Console.WriteLine("\nПлан носит информационный характер. При боли, травмах или хронических заболеваниях проконсультируйтесь со специалистом.");
}
