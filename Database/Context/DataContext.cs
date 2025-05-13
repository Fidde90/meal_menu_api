using meal_menu_api.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace meal_menu_api.Database.Context
{
    public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<AppUser>(options)
    {

        public DbSet<RecipeEntity> Recipes { get; set; }

        public DbSet<IngredientEntity> Ingredients { get; set; }

        public DbSet<StepEntity> Steps { get; set; }

        public DbSet<ImageEntity> Images { get; set; }

        public DbSet<UnitEntity> Units { get; set; }

        public DbSet<DinnerScheduleEntity> DinnerSchedules { get; set; }

        public DbSet<DinnerEntity> Dinners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Recipes -> Users: Delete Recipes when User is deleted
            modelBuilder.Entity<RecipeEntity>()
                .HasOne(r => r.User)
                .WithMany(u => u.Recipes)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade); // User deleted => Recipes deleted

            // Recipe -> Ingredients: Delete Ingredients when Recipe is deleted
            modelBuilder.Entity<IngredientEntity>()
                .HasOne(i => i.Recipe)
                .WithMany(r => r.Ingredients)
                .HasForeignKey(i => i.RecipeId)
                .OnDelete(DeleteBehavior.Cascade); // Recipe deleted => Ingredients deleted

            // Recipe -> Steps: Delete Steps when Recipe is deleted
            modelBuilder.Entity<StepEntity>()
                .HasOne(s => s.Recipe)
                .WithMany(r => r.Steps)
                .HasForeignKey(s => s.RecipeId)
                .OnDelete(DeleteBehavior.Cascade); // Recipe deleted => Steps deleted

            // Recipe -> Images: Delete Images when Recipe is deleted
            modelBuilder.Entity<ImageEntity>()
                .HasOne(img => img.Recipe)
                .WithMany(r => r.Images)
                .HasForeignKey(img => img.RecipeId)
                .OnDelete(DeleteBehavior.Cascade); // Recipe deleted => Images deleted

            // Ingredient -> Unit (DO NOT cascade delete here!)
            modelBuilder.Entity<IngredientEntity>()
                .HasOne(i => i.Unit)
                .WithMany()
                .HasForeignKey(i => i.UnitId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict delete of Unit when Ingredient is deleted

            // DinnerSchedule → Dinners = Cascade delete
            modelBuilder.Entity<DinnerEntity>()
                .HasOne(d => d.DinnerSchedule)
                .WithMany(ds => ds.Dinners)
                .HasForeignKey(d => d.DinnerScheduleId)
                .OnDelete(DeleteBehavior.Cascade); // DinnerSchedule deleted => Dinners deleted

            // Dinner → Recipe = SetNull delete
            modelBuilder.Entity<DinnerEntity>()
                .HasOne(d => d.Recipe)
                .WithMany()
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.SetNull); // Recipe deleted => Dinner Recipe set to null

            // Ensure there is no cascading delete conflict with Dinner -> Recipe relationship
            modelBuilder.Entity<DinnerEntity>()
                .HasOne(d => d.Recipe)
                .WithMany()
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.NoAction); // Avoid multiple cascade paths

            base.OnModelCreating(modelBuilder);
        }
    }
}
