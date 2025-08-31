using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Vahallan_Ingredient_Aggregator.Models.Components;
using Vahallan_Ingredient_Aggregator.Models.Notifications;
using Vahallan_Ingredient_Aggregator.Models.Pantry;
using Vahallan_Ingredient_Aggregator.Models.Photo;
using Vahallan_Ingredient_Aggregator.Models.Components;


namespace Vahallan_Ingredient_Aggregator.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<PantryItem> PantryItems { get; set; }
        public DbSet<RecipePhoto> RecipePhotos { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationPreference> NotificationPreferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure inheritance strategy
            modelBuilder.Entity<BaseIngredientComponent>()
           .HasDiscriminator<string>("Type")
           .HasValue<Ingredient>("Ingredient")
           .HasValue<Recipe>("Recipe")
           .HasValue<MealPlan>("MealPlan");

            // Configure base component photo relationship
            modelBuilder.Entity<BaseIngredientComponent>()
                .HasMany(e => e.Photos)
                .WithOne()
                .HasForeignKey(p => p.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<IdentityRole>().HasData(
    new IdentityRole
    {
        Id = "1",
        Name = "Admin",
        NormalizedName = "ADMIN",
        ConcurrencyStamp = Guid.NewGuid().ToString()
    },
    new IdentityRole
    {
        Id = "2",
        Name = "User",
        NormalizedName = "USER",
        ConcurrencyStamp = Guid.NewGuid().ToString()
    }
);
            var hasher = new PasswordHasher<IdentityUser>();
            var adminUser = new IdentityUser
            {
                Id = "admin-user-id",
                UserName = "admin@yourapp.com",
                NormalizedUserName = "ADMIN@YOURAPP.COM",
                Email = "admin@yourapp.com",
                NormalizedEmail = "ADMIN@YOURAPP.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123!");

            modelBuilder.Entity<IdentityUser>().HasData(adminUser);

            // Seed Admin Role Assignment
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "1", // Admin role ID
                    UserId = "admin-user-id"
                }
            );


            // Configure RecipeIngredient relationship
            modelBuilder.Entity<RecipeIngredient>(entity =>
            {
                entity.HasKey(ri => ri.Id);

                entity.HasOne(ri => ri.Recipe)
                    .WithMany(r => r.RecipeIngredients)
                    .HasForeignKey(ri => ri.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ri => ri.Ingredient)
                    .WithMany(i => i.RecipeIngredients)
                    .HasForeignKey(ri => ri.IngredientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(ri => ri.Quantity)
                    .HasColumnType("decimal(18,4)");

                entity.Property(ri => ri.Unit)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            // Configure Recipe
            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Instructions).IsRequired();
                entity.Property(e => e.NumberOfServings).HasColumnType("decimal(18,2)");

 

            });

            // Configure Ingredient
            modelBuilder.Entity<Ingredient>(entity =>
            {
  
                    entity.Property(e => e.Unit).IsRequired().HasMaxLength(50);
                    entity.Property(e => e.Quantity).HasColumnType("decimal(18,4)");
                    entity.Property(e => e.CostPerPackage).HasColumnType("decimal(18,2)");
                    entity.Property(e => e.CaloriesPerServing).HasColumnType("decimal(10,2)");
                    entity.Property(e => e.ServingsPerPackage).HasColumnType("decimal(18,2)");
                    // ServingCost is not mapped as it's a computed property
                

                // Make the relationships explicit
                entity.HasMany(i => i.RecipeIngredients)
                    .WithOne(ri => ri.Ingredient)
                    .HasForeignKey(ri => ri.IngredientId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            /////////////////////////////



            // Configure MealPlanRecipe relationship
            modelBuilder.Entity<MealPlanRecipe>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(mpr => mpr.MealPlan)
                    .WithMany(mp => mp.MealPlanRecipes)
                    .HasForeignKey(mpr => mpr.MealPlanId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(mpr => mpr.Recipe)
                    .WithMany()
                    .HasForeignKey(mpr => mpr.RecipeId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.Property(e => e.PrepareOnDate)
                    .IsRequired();

                entity.Property(e => e.ServingSize)
                    .IsRequired();

                entity.HasIndex(e => new { e.MealPlanId, e.RecipeId, e.PrepareOnDate })
                    .IsUnique();
            });
            // Configure PantryItem decimal properties
            modelBuilder.Entity<PantryItem>(entity =>
            {
                entity.Property(e => e.CurrentQuantity).HasColumnType("decimal(18,4)");
                entity.Property(e => e.LastPurchaseQuantity).HasColumnType("decimal(18,4)");
                entity.Property(e => e.LastPurchasePrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.DailyUsageRate).HasColumnType("decimal(18,4)");
                entity.Property(e => e.LowStockThreshold).HasColumnType("decimal(18,4)");
                entity.HasIndex(e => e.ExpirationDate);
            });



            // Configure NotificationPreference decimal properties
            modelBuilder.Entity<NotificationPreference>(entity =>
            {
                entity.Property(e => e.DefaultLowStockThreshold).HasColumnType("decimal(18,4)");
                entity.HasIndex(e => e.UserId);
            });

            //// Configure RecipePhoto required properties
            //modelBuilder.Entity<RecipePhoto>(entity =>
            //{
            //    entity.Property(e => e.FilePath).IsRequired();
            //    entity.HasOne<Recipe>()
            //        .WithMany()
            //        .HasForeignKey(p => p.RecipeId)
            //        .OnDelete(DeleteBehavior.Cascade);
            //});
            // Configure RecipePhoto
            modelBuilder.Entity<RecipePhoto>(entity =>
            {
                entity.Property(e => e.FilePath).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.ThumbnailPath).HasMaxLength(1000);
                entity.Property(e => e.FileName).HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.ContentType).IsRequired();
                entity.Property(e => e.UploadedById).IsRequired();
            });


            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Tags)
                .WithMany(t => t.Recipes)
                .UsingEntity(j => j.ToTable("RecipeTags"));

            

            RecipeSeeder.SeedCapreseSalad(modelBuilder);
        }
    }
}