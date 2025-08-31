using Microsoft.EntityFrameworkCore;
using Vahallan_Ingredient_Aggregator.Models.Photo;
using Vahallan_Ingredient_Aggregator.Models.Components;


public static class RecipeSeeder
{
    public static void SeedCapreseSalad(ModelBuilder modelBuilder)
    {
        // Setup photo directories
        var recipePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "recipe-photos");
        var originalsPath = Path.Combine(recipePath, "originals");
        var thumbnailsPath = Path.Combine(recipePath, "thumbnails");

        Directory.CreateDirectory(originalsPath);
        Directory.CreateDirectory(thumbnailsPath);

        // Copy seed photos
        var sourcePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "SeedData", "caprese-main.jpg");
        var destinationPhotoPath = Path.Combine(originalsPath, "caprese-main.jpg");
        var destinationThumbnailPath = Path.Combine(thumbnailsPath, "caprese-main.jpg");

        if (File.Exists(sourcePhotoPath))
        {
            File.Copy(sourcePhotoPath, destinationPhotoPath, true);
            File.Copy(sourcePhotoPath, destinationThumbnailPath, true);
        }

        // Create base recipe
        var recipe = new Recipe
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
            IsPublic = true,
            NumberOfServings = 4,
            Type = "Recipe",
            Unit = "serving",
            Quantity = 4,
            StoredUnit = "serving",
            StoredQuantity = 4
        };

        // Create base ingredients
        var ingredients = new[]
        {
            new Ingredient
            {
                Id = 1,
                Name = "Fresh Mozzarella",
                CostPerPackage = 5.99m,
                ServingsPerPackage = 16,
                CaloriesPerServing = 70,
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                Type = "Ingredient",
                IsSystemIngredient = true,
                Unit = "oz",
                Quantity = 1,
                StoredUnit = "g",
                StoredQuantity = 28.35m
            },
            new Ingredient
            {
                Id = 2,
                Name = "Ripe Tomatoes",
                CostPerPackage = 3.00m,
                ServingsPerPackage = 4,
                CaloriesPerServing = 22,
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                Type = "Ingredient",
                IsSystemIngredient = true,
                Unit = "count",
                Quantity = 1,
                StoredUnit = "count",
                StoredQuantity = 1
            },
            new Ingredient
            {
                Id = 3,
                Name = "Fresh Basil Leaves",
                CostPerPackage = 2.99m,
                ServingsPerPackage = 30,
                CaloriesPerServing = 1,
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                Type = "Ingredient",
                IsSystemIngredient = true,
                Unit = "count",
                Quantity = 1,
                StoredUnit = "count",
                StoredQuantity = 1
            },
            new Ingredient
            {
                Id = 4,
                Name = "Extra Virgin Olive Oil",
                CostPerPackage = 8.99m,
                ServingsPerPackage = 33.8m,
                CaloriesPerServing = 120,
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                Type = "Ingredient",
                IsSystemIngredient = true,
                Unit = "tbsp",
                Quantity = 1,
                StoredUnit = "ml",
                StoredQuantity = 14.7868m
            },
            new Ingredient
            {
                Id = 5,
                Name = "Balsamic Vinegar",
                CostPerPackage = 5.99m,
                ServingsPerPackage = 16.9m,
                CaloriesPerServing = 14,
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                Type = "Ingredient",
                IsSystemIngredient = true,
                Unit = "tbsp",
                Quantity = 1,
                StoredUnit = "ml",
                StoredQuantity = 14.7868m
            },
            new Ingredient
            {
                Id = 6,
                Name = "Salt",
                CostPerPackage = 0.99m,
                ServingsPerPackage = 156,
                CaloriesPerServing = 0,
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                Type = "Ingredient",
                IsSystemIngredient = true,
                Unit = "tsp",
                Quantity = 1,
                StoredUnit = "ml",
                StoredQuantity = 4.92892m
            },
            new Ingredient
            {
                Id = 7,
                Name = "Black Pepper",
                CostPerPackage = 3.99m,
                ServingsPerPackage = 144,
                CaloriesPerServing = 0,
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                Type = "Ingredient",
                IsSystemIngredient = true,
                Unit = "tsp",
                Quantity = 1,
                StoredUnit = "ml",
                StoredQuantity = 4.92892m
            }
        };

        // Create recipe-ingredient relationships
        var recipeIngredients = new[]
        {
            new RecipeIngredient
            {
                Id = 1,
                RecipeId = recipe.Id,
                IngredientId = 1,
                Quantity = 16,
                Unit = "oz"
            },
            new RecipeIngredient
            {
                Id = 2,
                RecipeId = recipe.Id,
                IngredientId = 2,
                Quantity = 4,
                Unit = "count"
            },
            new RecipeIngredient
            {
                Id = 3,
                RecipeId = recipe.Id,
                IngredientId = 3,
                Quantity = 20,
                Unit = "count"
            },
            new RecipeIngredient
            {
                Id = 4,
                RecipeId = recipe.Id,
                IngredientId = 4,
                Quantity = 2,
                Unit = "tbsp"
            },
            new RecipeIngredient
            {
                Id = 5,
                RecipeId = recipe.Id,
                IngredientId = 5,
                Quantity = 2,
                Unit = "tbsp"
            },
            new RecipeIngredient
            {
                Id = 6,
                RecipeId = recipe.Id,
                IngredientId = 6,
                Quantity = 0.5m,
                Unit = "tsp"
            },
            new RecipeIngredient
            {
                Id = 7,
                RecipeId = recipe.Id,
                IngredientId = 7,
                Quantity = 0.25m,
                Unit = "tsp"
            }
        };

        // Create photos
        var photos = new[]
        {
            new RecipePhoto
            {
                Id = 1,
                RecipeId = recipe.Id,
                FilePath = "/recipe-photos/originals/caprese-main.jpg",
                ThumbnailPath = "/recipe-photos/thumbnails/caprese-main.jpg",
                ContentType = "image/jpeg",
                FileSize = 19661L,
                FileName = "caprese-main.jpg",
                Description = "Classic Caprese Salad with alternating slices of mozzarella and tomato",
                IsMain = true,
                IsApproved = true,
                UploadedById = "system",
                UploadedAt = DateTime.UtcNow
            },
            new RecipePhoto
            {
                Id = 2,
                RecipeId = recipe.Id,
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

        // Seed all entities
        modelBuilder.Entity<Recipe>().HasData(recipe);
        modelBuilder.Entity<Ingredient>().HasData(ingredients);
        modelBuilder.Entity<RecipeIngredient>().HasData(recipeIngredients);
        modelBuilder.Entity<RecipePhoto>().HasData(photos);
    }
}