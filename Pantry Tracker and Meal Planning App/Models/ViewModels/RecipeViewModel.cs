namespace Vahallan_Ingredient_Aggregator.Models.ViewModels
{
    public class RecipeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Keep these for backward compatibility during transition
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public int TotalTime { get; set; }

        // Wallpaper-specific properties
        public string Collection { get; set; } = string.Empty;
        public decimal StandardSheetSize { get; set; } = 25m;
        public RecipeAccuracyLevel AccuracyLevel { get; set; }

        // Display properties
        public string MainPhotoUrl { get; set; }
        public int IngredientsCount { get; set; }

        // Remove these - no more public/private distinction
        public bool IsPublic { get; set; } = true; // Always true now
        public bool IsOwner { get; set; }
        public string CreatedBy { get; set; }

        // Remove these external properties if not needed
        //public string ExternalSource { get; set; }
        //public string ExternalId { get; set; }
        //public bool IsExternal => !string.IsNullOrEmpty(ExternalSource);
        public bool ShowInIngredientsList { get; set; } = false; // ADD THIS


    }

    // Add the enum if it doesn't exist elsewhere
    public enum RecipeAccuracyLevel
    {
        Estimate = 0,
        Tested = 1,
        Verified = 2,
        Professional = 3
    }
}