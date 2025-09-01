using Microsoft.EntityFrameworkCore;
using Vahallan_Ingredient_Aggregator.Models.Components;
using Vahallan_Ingredient_Aggregator.Models.Photo;

public static class RecipeSeeder
{
    public static void SeedEdenWallpaper(ModelBuilder modelBuilder)
    {
        // Ensure directories exist
        var recipePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "recipe-photos");
        var originalsPath = Path.Combine(recipePath, "originals");
        var thumbnailsPath = Path.Combine(recipePath, "thumbnails");

        Directory.CreateDirectory(originalsPath);
        Directory.CreateDirectory(thumbnailsPath);

        // Copy seed photo files if they don't exist
        var sourcePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "SeedData", "eden-main.jpg");
        var destinationPhotoPath = Path.Combine(originalsPath, "eden-main.jpg");
        var destinationThumbnailPath = Path.Combine(thumbnailsPath, "eden-main.jpg");

        if (File.Exists(sourcePhotoPath))
        {
            File.Copy(sourcePhotoPath, destinationPhotoPath, true);
            File.Copy(sourcePhotoPath, destinationThumbnailPath, true);
        }

        // Seed the Eden wallpaper pattern
        var edenPattern = new Recipe
        {
            Id = 100,
            Name = "Eden",
            Description = "Luxurious textured wallpaper with metallic accents and organic cork elements, creating an enchanted forest ambiance.",
            Instructions = @"1. Prepare the wall surface by cleaning and priming if necessary.
2. Apply base coat of Off-White Egg latex paint using roller in even strokes.
3. Allow base coat to dry completely (4-6 hours).
4. Apply Pearl metallic paint using sea sponge in circular motions for texture.
5. While Pearl is still tacky, lightly dab Champagne metallic accents.
6. Sprinkle White Gold glitter sparingly over wet metallic areas.
7. Press cork pieces into designated areas while paint is workable.
8. Allow to dry overnight before applying Polycrylic sealer with brush.
9. Apply thin, even coat of sealer avoiding drips or runs.
10. Allow 24 hours cure time before normal use.",
            PrepTimeMinutes = 45,
            CookTimeMinutes = 0, // No "cooking" time for wallpaper
            CreatedById = "system",
            CreatedAt = DateTime.UtcNow,
            Version = 1,
            Unit = "sq ft",
            Quantity = 100, // Standard coverage area
            Type = "Recipe",
           // StoredUnit = "sq ft",
          //  StoredQuantity = 100,
            IsPublic = true,
            StandardSheetSize = 25, // Square feet coverage

            // Wallpaper-specific properties
            Collection = "Enchanted Collection",
            ShowInIngredientsList = false,
            AccuracyLevel = RecipeAccuracyLevel.Tested,
            PatternCode = "EDEN-001",
            //StandardSquareFeet = 100m
        };

        // Seed wallpaper materials
        var materials = new[]
        {
            new Ingredient
            {
                Id = 101,
                Name = "Off-White Egg Latex Paint",
                Quantity = 3, // Current inventory: 3 gallons
                Unit = "gallons",
                CostPerPackage = 45.99m, // Cost per gallon
                UnitsPerPackage = 1, // 1 gallon per container
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                StoredQuantity = 3,
                StoredUnit = "gallons",
                Type = "Ingredient",
                MaterialType = "Paint",
                Vendor = "Pittsburgh Paints"
            },
            new Ingredient
            {
                Id = 102,
                Name = "Pearl Metallic Paint",
                Quantity = 2, // Current inventory: 2 quarts
                Unit = "quarts",
                CostPerPackage = 24.95m, // Cost per quart
                UnitsPerPackage = 1, // 1 quart per container
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                StoredQuantity = 2,
                StoredUnit = "quarts",
                Type = "Ingredient",
                MaterialType = "Metallic Paint",
                Vendor = "Modern Masters"
            },
            new Ingredient
            {
                Id = 103,
                Name = "Champagne Metallic Paint",
                Quantity = 1.5m, // Current inventory: 1.5 quarts
                Unit = "quarts",
                CostPerPackage = 26.50m, // Cost per quart
                UnitsPerPackage = 1, // 1 quart per container
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                StoredQuantity = 1.5m,
                StoredUnit = "quarts",
                Type = "Ingredient",
                MaterialType = "Metallic Paint",
                Vendor = "Modern Masters"
            },
            new Ingredient
            {
                Id = 104,
                Name = "Polycrylic Semi-Gloss Sealer",
                Quantity = 4, // Current inventory: 4 quarts
                Unit = "quarts",
                CostPerPackage = 18.75m, // Cost per quart
                UnitsPerPackage = 1, // 1 quart per container
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                StoredQuantity = 4,
                StoredUnit = "quarts",
                Type = "Ingredient",
                MaterialType = "Sealer",
                Vendor = "Pittsburgh Paints"
            },
            new Ingredient
            {
                Id = 105,
                Name = "Natural Cork Pieces",
                Quantity = 15, // Current inventory: 15 lbs
                Unit = "lbs",
                CostPerPackage = 89.99m, // Cost per 20 lb bag
                UnitsPerPackage = 20, // 20 lbs per bag
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                StoredQuantity = 15,
                StoredUnit = "lbs",
                Type = "Ingredient",
                MaterialType = "Texture",
                Vendor = "Cork Supply Co"
            },
            new Ingredient
            {
                Id = 106,
                Name = "White Gold Glitter",
                Quantity = 8, // Current inventory: 8 oz
                Unit = "oz",
                CostPerPackage = 12.95m, // Cost per 4 oz container
                UnitsPerPackage = 4, // 4 oz per container
                CreatedById = "system",
                CreatedAt = DateTime.UtcNow,
                StoredQuantity = 8,
                StoredUnit = "oz",
                Type = "Ingredient",
                MaterialType = "Glitter",
                Vendor = "Craft Essentials"
            }
        };

        // Seed recipe photos
        var recipePhotos = new[]
        {
            new RecipePhoto
            {
                Id = 101,
                RecipeId = edenPattern.Id,
                FilePath = "/recipe-photos/originals/eden-main.jpg",
                ThumbnailPath = "/recipe-photos/thumbnails/eden-main.jpg",
                ContentType = "image/jpeg",
                FileSize = 25000L,
                FileName = "eden-main.jpg",
                Description = "Eden wallpaper pattern showing metallic texture with cork accents",
                IsMain = true,
                IsApproved = true,
                UploadedById = "system",
                UploadedAt = DateTime.UtcNow
            },
            new RecipePhoto
            {
                Id = 102,
                RecipeId = edenPattern.Id,
                FilePath = "/recipe-photos/originals/eden-detail.jpg",
                ThumbnailPath = "/recipe-photos/thumbnails/eden-detail.jpg",
                ContentType = "image/jpeg",
                FileSize = 18500L,
                FileName = "eden-detail.jpg",
                Description = "Close-up detail of Eden pattern texture and metallic finish",
                IsMain = false,
                IsApproved = true,
                UploadedById = "system",
                UploadedAt = DateTime.UtcNow
            }
        };

        // Recipe-Material relationships (amounts needed per 100 sq ft)
        var recipeIngredients = new[]
        {
            new RecipeIngredient
            {
                Id = 101,
                RecipeId = edenPattern.Id,
                IngredientId = 101, // Off-White Egg Latex Paint
                Quantity = 0.75m, // 3/4 gallon per 100 sq ft
                Unit = "gallons"
            },
            new RecipeIngredient
            {
                Id = 102,
                RecipeId = edenPattern.Id,
                IngredientId = 102, // Pearl Metallic Paint
                Quantity = 0.5m, // 1/2 quart per 100 sq ft
                Unit = "quarts"
            },
            new RecipeIngredient
            {
                Id = 103,
                RecipeId = edenPattern.Id,
                IngredientId = 103, // Champagne Metallic Paint
                Quantity = 0.25m, // 1/4 quart per 100 sq ft
                Unit = "quarts"
            },
            new RecipeIngredient
            {
                Id = 104,
                RecipeId = edenPattern.Id,
                IngredientId = 104, // Polycrylic Sealer
                Quantity = 0.33m, // 1/3 quart per 100 sq ft
                Unit = "quarts"
            },
            new RecipeIngredient
            {
                Id = 105,
                RecipeId = edenPattern.Id,
                IngredientId = 105, // Natural Cork Pieces
                Quantity = 2.5m, // 2.5 lbs per 100 sq ft
                Unit = "lbs"
            },
            new RecipeIngredient
            {
                Id = 106,
                RecipeId = edenPattern.Id,
                IngredientId = 106, // White Gold Glitter
                Quantity = 1, // 1 oz per 100 sq ft
                Unit = "oz"
            }
        };

        // Configure the entity seeding
        modelBuilder.Entity<Recipe>().HasData(edenPattern);
        modelBuilder.Entity<Ingredient>().HasData(materials);
        modelBuilder.Entity<RecipeIngredient>().HasData(recipeIngredients);
        modelBuilder.Entity<RecipePhoto>().HasData(recipePhotos);
    }
}