using meal_menu_api.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace meal_menu_api.Database.Context
{
    public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<AppUser>(options)
    {

        public DbSet<RecipeEntity> Recipes { get; set; }

        public DbSet<IngredientEntity> Ingredients { get; set; }

        public DbSet<StepEntity> Steps { get; set; }

        public DbSet<ImageEntity> Images { get; set; }

        public DbSet<UnitEntity> Units { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Recipes -> Users
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

            // Ingredient -> Unit (DO NOT cascade delete here!)
            modelBuilder.Entity<IngredientEntity>()
                .HasOne(i => i.Unit)
                .WithMany() // no navigation back needed
                .HasForeignKey(i => i.UnitId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of a unit if used
        }
    }
}
