using FitnessPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessPlanner.Data;

public sealed class FitnessDbContext(DbContextOptions<FitnessDbContext> options) : DbContext(options)
{
    public DbSet<FitnessProfile> FitnessProfiles => Set<FitnessProfile>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();
    public DbSet<WorkoutDay> WorkoutDays => Set<WorkoutDay>();
    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FitnessProfile>()
            .Property(profile => profile.Name)
            .HasMaxLength(100)
            .IsRequired();
        modelBuilder.Entity<FitnessProfile>()
            .Property(profile => profile.Equipment)
            .HasMaxLength(300);
        modelBuilder.Entity<FitnessProfile>().Property(profile => profile.Goal).HasConversion<string>();
        modelBuilder.Entity<FitnessProfile>().Property(profile => profile.Level).HasConversion<string>();

        modelBuilder.Entity<Exercise>()
            .Property(exercise => exercise.Name)
            .HasMaxLength(120)
            .IsRequired();
        modelBuilder.Entity<Exercise>().Property(exercise => exercise.MuscleGroup).HasConversion<string>();
        modelBuilder.Entity<Exercise>().Property(exercise => exercise.Difficulty).HasConversion<string>();

        modelBuilder.Entity<WorkoutPlan>()
            .HasOne(plan => plan.Profile)
            .WithMany(profile => profile.Plans)
            .HasForeignKey(plan => plan.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<WorkoutPlan>().Property(plan => plan.Goal).HasConversion<string>();
        modelBuilder.Entity<WorkoutDay>()
            .HasOne(day => day.Plan)
            .WithMany(plan => plan.WorkoutDays)
            .HasForeignKey(day => day.PlanId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<WorkoutExercise>()
            .HasOne(item => item.WorkoutDay)
            .WithMany(day => day.WorkoutExercises)
            .HasForeignKey(item => item.WorkoutDayId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<WorkoutExercise>()
            .HasOne(item => item.Exercise)
            .WithMany()
            .HasForeignKey(item => item.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
