using meal_menu_api.Entities.Dinners;
using meal_menu_api.Entities.Account;
using meal_menu_api.Entities.Recipes;
using meal_menu_api.Models.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using meal_menu_api.Entities;
using meal_menu_api.Entities.ShoppingList;
using meal_menu_api.Entities.Groups;

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

        public DbSet<GroupEntity> Groups { get; set; }

        public DbSet<GroupMemberEntity> GroupMembers { get; set; }

        public DbSet<GroupInvitationEntity> GroupInvitations { get; set; }

        public DbSet<GroupRecipeEntity> GroupRecipes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new ValueConverter<GroupRole, string>(
                v => v.ToString(),    // Från enum till string (vid sparande)
                v => (GroupRole)Enum.Parse(typeof(GroupRole), v) // Från string till enum (vid läsning)
            );

            modelBuilder.Entity<GroupMemberEntity>()
                .Property(e => e.Role)
                .HasConversion(converter);


            // USER → RECIPES
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.Recipes)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // USER → DINNER SCHEDULES
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.DinnerSchedules)
                .WithOne(ds => ds.User)
                .HasForeignKey(ds => ds.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // USER → SHOPPING LISTS
            modelBuilder.Entity<AppUser>()
                .HasMany(u => u.ShoppingLists)
                .WithOne(sl => sl.User)
                .HasForeignKey(sl => sl.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // RECIPE → INGREDIENTS
            modelBuilder.Entity<RecipeEntity>()
                .HasMany(r => r.Ingredients)
                .WithOne(i => i.Recipe)
                .HasForeignKey(i => i.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // RECIPE → IMAGES
            modelBuilder.Entity<RecipeEntity>()
                .HasMany(r => r.Images)
                .WithOne(img => img.Recipe)
                .HasForeignKey(img => img.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // RECIPE → STEPS
            modelBuilder.Entity<RecipeEntity>()
                .HasMany(r => r.Steps)
                .WithOne(s => s.Recipe)
                .HasForeignKey(s => s.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            // DINNER SCHEDULE → DINNER ENTITIES
            modelBuilder.Entity<DinnerScheduleEntity>()
                .HasMany(ds => ds.Dinners)
                .WithOne(d => d.DinnerSchedule)
                .HasForeignKey(d => d.DinnerScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            // DINNER SCHEDULE → SHOPPING LIST (en-till-en)
            modelBuilder.Entity<DinnerScheduleEntity>()
                .HasOne(ds => ds.ShoppingList)
                .WithOne(sl => sl.DinnerSchedule)
                .HasForeignKey<ShoppingListEntity>(sl => sl.DinnerScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            // SHOPPING LIST → SHOPPING LIST INGREDIENTS
            modelBuilder.Entity<ShoppingListEntity>()
                .HasMany(sl => sl.Ingredients)
                .WithOne(sli => sli.ShoppingList)
                .HasForeignKey(sli => sli.ShoppingListId)
                .OnDelete(DeleteBehavior.Cascade);


            // KOPPLINGSTABELL FÖR ANVÄNDARE OCH GRUPPER
            modelBuilder.Entity<GroupMemberEntity>()
                .HasKey(gm => new { gm.GroupId, gm.UserId });

            modelBuilder.Entity<GroupMemberEntity>()
                .HasOne(gm => gm.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(gm => gm.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupMemberEntity>()
                .HasOne(gm => gm.User)
                .WithMany(u => u.GroupMemberships)
                .HasForeignKey(gm => gm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //INBJUDNINGAR
            modelBuilder.Entity<GroupInvitationEntity>()
                .HasOne(i => i.Group)
                .WithMany(g => g.Invitations)
                .HasForeignKey(i => i.GroupId);

            modelBuilder.Entity<GroupInvitationEntity>()
                .HasOne(i => i.InvitedUser)
                .WithMany()
                .HasForeignKey(i => i.InvitedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GroupInvitationEntity>()
                .HasOne(i => i.InvitedByUser)
                .WithMany()
                .HasForeignKey(i => i.InvitedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            //GRUPPRECEPT
            modelBuilder.Entity<GroupRecipeEntity>()
                .HasOne(gr => gr.Recipe)
                .WithMany(r => r.SharedWithGroups)
                .HasForeignKey(gr => gr.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupRecipeEntity>()
                .HasOne(gr => gr.SharedByUser)
                .WithMany()
                .HasForeignKey(gr => gr.SharedByUserId)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }
    }
}
