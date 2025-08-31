using Microsoft.EntityFrameworkCore;
using Vahallan_Ingredient_Aggregator.Models.Components;
using Vahallan_Ingredient_Aggregator.Models.Photo;

public static class RecipeSeeder
{
    public static void SeedCapreseSalad(ModelBuilder modelBuilder)
    {
        // Ensure directories exist
        var recipePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "recipe-photos");
        var originalsPath = Path.Combine(recipePath, "originals");
        var thumbnailsPath = Path.Combine(recipePath, "thumbnails");

        Directory.CreateDirectory(originalsPath);
        Directory.CreateDirectory(thumbnailsPath);

        // Copy seed photo files if they don't exist
        var sourcePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "SeedData", "caprese-main.jpg");
        var destinationPhotoPath = Path.Combine(originalsPath, "caprese-main.jpg");
        var destinationThumbnailPath = Path.Combine(thumbnailsPath, "caprese-main.jpg");

        if (File.Exists(sourcePhotoPath))
        {
            File.Copy(sourcePhotoPath, destinationPhotoPath, true);
            File.Copy(sourcePhotoPath, destinationThumbnailPath, true); // For demo, using same file as thumbnail
        }

        // Seed the recipe first - ADD MISSING PROPERTIES
        var capreseSalad = new Recipe
        {
            Id = 8,
            Name = "Classic Caprese Salad",
            Description = "A simple and elegant Italian salad made with fresh mozzarella, tomatoes, and basil.",
            Instructions = @"1. Slice the mozzarella and tomatoes into 1/4-inch thick slices.
2. On a serving plate, alternately arrange the mozzarella and tomato slices in a circular pattern.
3. Tuck fresh basil leaves between the mozzarella and tomato slices.
4. Drizzle with extra virgin olive oil and balsamic vinegar.
5. Season with salt and freshly ground black pepper.
6. Serve immediately at room temperature.",
            PrepTimeMinutes = 15,
            CookTimeMinutes = 0,
            CreatedById = "system",
            CreatedAt = DateTime.UtcNow,
            Version = 1,
            Unit = "serving",
            Quantity = 4,
            Type = "Recipe",
            StoredUnit = "serving",
            StoredQuantity = 4,
            IsPublic = true,

            // ADD THESE REQUIRED WALLPAPER PROPERTIES
            Collection = "Sample Recipes",
            ShowInIngredientsList = false,
            AccuracyLevel = RecipeAccuracyLevel.Tested,
            PatternCode = "CAPRESE-001",  // THIS WAS MISSING!
            StandardSquareFeet = 100m     // DEFAULT VALUE
        };

        // Seed base ingredients - REMOVE IsSystemIngredient and IsPromoted
        var ingredients = new[]
        {
            new Ingredient
            {
                Id = 1,
                Name = "Fresh Mozzarella",
                Quantity = 16,
                Unit = "oz",
                CostPerPackage = 5.99m,
                ServingsPerPackage = 16,
                CaloriesPerServing = 70,
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                StoredQuantity = 453.592m,
                StoredUnit = "g",
                Type = "Ingredient",
                MaterialType = "Dairy",  // ADD wallpaper property
                Vendor = "Local Farm"    // ADD wallpaper property
                // REMOVED: IsSystemIngredient and IsPromoted
                // REMOVED: ParentId (not used in your current model)
            },
            new Ingredient
            {
                Id = 2,
                Name = "Ripe Tomatoes",
                Quantity = 4,
                Unit = "count",
                CostPerPackage = 3.00m,
                ServingsPerPackage = 4,
                CaloriesPerServing = 22,
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                StoredQuantity = 4,
                StoredUnit = "count",
                Type = "Ingredient",
                MaterialType = "Produce",
                Vendor = "Local Market"
                // REMOVED: IsSystemIngredient, IsPromoted, ParentId
            },
            new Ingredient
            {
                Id = 3,
                Name = "Fresh Basil Leaves",
                Quantity = 20,
                Unit = "count",
                CostPerPackage = 2.99m,
                ServingsPerPackage = 30,
                CaloriesPerServing = 1,
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                StoredQuantity = 20,
                StoredUnit = "count",
                Type = "Ingredient",
                MaterialType = "Herbs",
                Vendor = "Garden Center"
                // REMOVED: IsSystemIngredient, IsPromoted, ParentId
            },
            new Ingredient
            {
                Id = 4,
                Name = "Extra Virgin Olive Oil",
                Quantity = 2,
                Unit = "tbsp",
                CostPerPackage = 8.99m,
                ServingsPerPackage = 33.8m,
                CaloriesPerServing = 120,
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                StoredQuantity = 29.5735m,
                StoredUnit = "ml",
                Type = "Ingredient",
                MaterialType = "Oil",
                Vendor = "Mediterranean Imports"
                // REMOVED: IsSystemIngredient, IsPromoted, ParentId
            },
            new Ingredient
            {
                Id = 5,
                Name = "Balsamic Vinegar",
                Quantity = 2,
                Unit = "tbsp",
                CostPerPackage = 5.99m,
                ServingsPerPackage = 16.9m,
                CaloriesPerServing = 14,
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                StoredQuantity = 29.5735m,
                StoredUnit = "ml",
                Type = "Ingredient",
                MaterialType = "Condiment",
                Vendor = "Mediterranean Imports"
                // REMOVED: IsSystemIngredient, IsPromoted, ParentId
            },
            new Ingredient
            {
                Id = 6,
                Name = "Salt",
                Quantity = 0.5m,
                Unit = "tsp",
                CostPerPackage = 0.99m,
                ServingsPerPackage = 156,
                CaloriesPerServing = 0,
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                StoredQuantity = 2.46446m,
                StoredUnit = "ml",
                Type = "Ingredient",
                MaterialType = "Seasoning",
                Vendor = "General Store"
                // REMOVED: IsSystemIngredient, IsPromoted, ParentId
            },
            new Ingredient
            {
                Id = 7,
                Name = "Black Pepper",
                Quantity = 0.25m,
                Unit = "tsp",
                CostPerPackage = 3.99m,
                ServingsPerPackage = 144,
                CaloriesPerServing = 0,
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                StoredQuantity = 1.23223m,
                StoredUnit = "ml",
                Type = "Ingredient",
                MaterialType = "Seasoning",
                Vendor = "Spice Company"
                // REMOVED: IsSystemIngredient, IsPromoted, ParentId
            }
        };

        // Seed recipe photos
        var recipePhotos = new[]
        {
            new RecipePhoto
            {
                Id = 1,
                RecipeId = capreseSalad.Id,
                FilePath = "/recipe-photos/originals/caprese-main.jpg",
                ThumbnailPath = "/recipe-photos/thumbnails/caprese-main.jpg",
                ContentType = "image/jpeg",
                FileSize = 20L,
                FileName = "caprese-main.jpg",
                Description = "Classic Caprese Salad with alternating slices of mozzarella and tomato",
                IsMain = true,  // CORRECTED: Use IsMain instead of IsMainPhoto
                IsApproved = true,
                UploadedById = "system",
                UploadedAt = DateTime.UtcNow
            },
            new RecipePhoto
            {
                Id = 2,
                RecipeId = capreseSalad.Id,
                FilePath = "/recipe-photos/originals/caprese-salad-recipe-1.jpg",
                ThumbnailPath = "/recipe-photos/thumbnails/caprese-salad-recipe-1.jpg",
                ContentType = "image/jpeg",
                FileSize = 18432L,
                FileName = "caprese-salad-recipe-1.jpg",
                Description = "Caprese Salad from a different angle",
                IsMain = false,
                IsApproved = true,
                UploadedById = "system",
                UploadedAt = DateTime.UtcNow
            }
        };

        // ADD MISSING: Seed RecipeIngredient relationships
        var recipeIngredients = new[]
        {
            new RecipeIngredient
            {
                Id = 1,
                RecipeId = capreseSalad.Id,
                IngredientId = 1, // Fresh Mozzarella
                Quantity = 8,
                Unit = "oz"
            },
            new RecipeIngredient
            {
                Id = 2,
                RecipeId = capreseSalad.Id,
                IngredientId = 2, // Ripe Tomatoes
                Quantity = 2,
                Unit = "count"
            },
            new RecipeIngredient
            {
                Id = 3,
                RecipeId = capreseSalad.Id,
                IngredientId = 3, // Fresh Basil Leaves
                Quantity = 10,
                Unit = "count"
            },
            new RecipeIngredient
            {
                Id = 4,
                RecipeId = capreseSalad.Id,
                IngredientId = 4, // Extra Virgin Olive Oil
                Quantity = 2,
                Unit = "tbsp"
            },
            new RecipeIngredient
            {
                Id = 5,
                RecipeId = capreseSalad.Id,
                IngredientId = 5, // Balsamic Vinegar
                Quantity = 1,
                Unit = "tbsp"
            },
            new RecipeIngredient
            {
                Id = 6,
                RecipeId = capreseSalad.Id,
                IngredientId = 6, // Salt
                Quantity = 0.25m,
                Unit = "tsp"
            },
            new RecipeIngredient
            {
                Id = 7,
                RecipeId = capreseSalad.Id,
                IngredientId = 7, // Black Pepper
                Quantity = 0.125m,
                Unit = "tsp"
            }
        };

        // Configure the entity seeding
        modelBuilder.Entity<Recipe>().HasData(capreseSalad);
        modelBuilder.Entity<Ingredient>().HasData(ingredients);
        modelBuilder.Entity<RecipeIngredient>().HasData(recipeIngredients); // ADD THIS
        modelBuilder.Entity<RecipePhoto>().HasData(recipePhotos);
    }
}