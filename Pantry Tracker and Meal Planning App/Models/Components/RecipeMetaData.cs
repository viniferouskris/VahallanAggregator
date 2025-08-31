namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.Components
{
    public class RecipeMetaData
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string CuisineType { get; set; }
        public string DifficultyLevel { get; set; }
        public decimal EstimatedCost { get; set; }
        public int ServingSize { get; set; }
        public string NutritionalInfo { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
