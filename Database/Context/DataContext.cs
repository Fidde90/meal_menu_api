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

        public DbSet<ShoppingListEntity> ShoppingLists { get; set; }

        public DbSet<ShoppingListIngredientEntity> ShoppingListIngredients { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Recipes -> Users: Delete Recipes when User is deleted
            modelBuilder.Entity<RecipeEntity>()
                .HasOne(r => r.User)
                .WithMany(u => u.Recipes)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Recipe -> Ingredients
            modelBuilder.Entity<IngredientEntity>()
                .HasOne(i => i.Recipe)
                .WithMany(r => r.Ingredients)
                .HasForeignKey(i => i.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Recipe -> Steps
            modelBuilder.Entity<StepEntity>()
                .HasOne(s => s.Recipe)
                .WithMany(r => r.Steps)
                .HasForeignKey(s => s.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Recipe -> Images
            modelBuilder.Entity<ImageEntity>()
                .HasOne(img => img.Recipe)
                .WithMany(r => r.Images)
                .HasForeignKey(img => img.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ingredient -> Unit: Restrict delete
            modelBuilder.Entity<IngredientEntity>()
                .HasOne(i => i.Unit)
                .WithMany()
                .HasForeignKey(i => i.UnitId)
                .OnDelete(DeleteBehavior.Restrict);

            // DinnerSchedule -> Dinners
            modelBuilder.Entity<DinnerEntity>()
                .HasOne(d => d.DinnerSchedule)
                .WithMany(ds => ds.Dinners)
                .HasForeignKey(d => d.DinnerScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Dinner -> Recipe: allow null if recipe is deleted
            modelBuilder.Entity<DinnerEntity>()
                .HasOne(d => d.Recipe)
                .WithMany()
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.SetNull); // ← detta räcker!

            // ShoppingList -> Ingredients
            modelBuilder.Entity<ShoppingListEntity>()
                .HasMany(s => s.Ingredients)
                .WithOne(i => i.ShoppingList)
                .HasForeignKey(i => i.ShoppingListId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShoppingListEntity>()
                .HasOne(s => s.DinnerSchedule)
                .WithMany(ds => ds.)
                .HasForeignKey(s => s.DinnerScheduleId)
                .OnDelete(DeleteBehavior.Cascade); // 👈 orsakar konflikten

            base.OnModelCreating(modelBuilder);
        }
    }
}
