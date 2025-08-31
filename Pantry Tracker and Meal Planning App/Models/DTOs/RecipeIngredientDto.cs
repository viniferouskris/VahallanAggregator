using System.Text.Json.Serialization;

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.DTOs
{
    public class RecipeIngredientDto
    {
        [JsonPropertyName("ingredientId")]
        public int IngredientId { get; set; }

        [JsonPropertyName("quantity")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public decimal Quantity { get; set; }

        [JsonPropertyName("unit")]
        public string Unit { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"IngredientId: {IngredientId}, Quantity: {Quantity}, Unit: {Unit}";
        }
    }
}