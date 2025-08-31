
namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.ViewModels
{
    public class ExternalRecipeViewModel
    {
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Area { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Source { get; set; }
        public bool IsImported { get; set; }
        public int? LocalRecipeId { get; set; }
    }

    public class ExternalRecipeDetailViewModel
    {
        public string ExternalId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Area { get; set; }
        public string Instructions { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Source { get; set; }
        public string VideoUrl { get; set; }
        public List<string> Ingredients { get; set; } = new();
        public List<string> Measurements { get; set; } = new();
        public bool IsImported { get; set; }
        public int? LocalRecipeId { get; set; }

        public IEnumerable<(string Ingredient, string Measurement)> GetIngredientPairs()
        {
            return Ingredients.Zip(Measurements, (i, m) => (i, m))
                            .Where(pair => !string.IsNullOrWhiteSpace(pair.i));
        }
    }

    public class ExternalRecipeSearchViewModel
    {
        public string SearchTerm { get; set; }
        public List<ExternalRecipeViewModel> Results { get; set; } = new();
        public string ErrorMessage { get; set; }
    }
}
