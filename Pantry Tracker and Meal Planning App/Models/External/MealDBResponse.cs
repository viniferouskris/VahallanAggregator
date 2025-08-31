using System.Text.Json.Serialization;

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.External
{
    public class MealDBResponse
    {
        [JsonPropertyName("meals")]
        public List<MealDBMeal> Meals { get; set; }
    }

    public class MealDBMeal
    {
        [JsonPropertyName("idMeal")]
        public string IdMeal { get; set; }

        [JsonPropertyName("strMeal")]
        public string StrMeal { get; set; }

        [JsonPropertyName("strCategory")]
        public string StrCategory { get; set; }

        [JsonPropertyName("strArea")]
        public string StrArea { get; set; }

        [JsonPropertyName("strInstructions")]
        public string StrInstructions { get; set; }

        [JsonPropertyName("strMealThumb")]
        public string StrMealThumb { get; set; }

        [JsonPropertyName("strTags")]
        public string StrTags { get; set; }

        // Ingredients (TheMealDB uses numbered properties)
        [JsonPropertyName("strIngredient1")]
        public string StrIngredient1 { get; set; }
        [JsonPropertyName("strIngredient2")]
        public string StrIngredient2 { get; set; }
        [JsonPropertyName("strIngredient3")]
        public string StrIngredient3 { get; set; }
        [JsonPropertyName("strIngredient4")]
        public string StrIngredient4 { get; set; }
        [JsonPropertyName("strIngredient5")]
        public string StrIngredient5 { get; set; }
        [JsonPropertyName("strIngredient6")]
        public string StrIngredient6 { get; set; }
        [JsonPropertyName("strIngredient7")]
        public string StrIngredient7 { get; set; }
        [JsonPropertyName("strIngredient8")]
        public string StrIngredient8 { get; set; }
        [JsonPropertyName("strIngredient9")]
        public string StrIngredient9 { get; set; }
        [JsonPropertyName("strIngredient10")]
        public string StrIngredient10 { get; set; }
        [JsonPropertyName("strIngredient11")]
        public string StrIngredient11 { get; set; }
        [JsonPropertyName("strIngredient12")]
        public string StrIngredient12 { get; set; }
        [JsonPropertyName("strIngredient13")]
        public string StrIngredient13 { get; set; }
        [JsonPropertyName("strIngredient14")]
        public string StrIngredient14 { get; set; }
        [JsonPropertyName("strIngredient15")]
        public string StrIngredient15 { get; set; }
        [JsonPropertyName("strIngredient16")]
        public string StrIngredient16 { get; set; }
        [JsonPropertyName("strIngredient17")]
        public string StrIngredient17 { get; set; }
        [JsonPropertyName("strIngredient18")]
        public string StrIngredient18 { get; set; }
        [JsonPropertyName("strIngredient19")]
        public string StrIngredient19 { get; set; }
        [JsonPropertyName("strIngredient20")]
        public string StrIngredient20 { get; set; }

        // Measurements
        [JsonPropertyName("strMeasure1")]
        public string StrMeasure1 { get; set; }
        [JsonPropertyName("strMeasure2")]
        public string StrMeasure2 { get; set; }
        [JsonPropertyName("strMeasure3")]
        public string StrMeasure3 { get; set; }
        [JsonPropertyName("strMeasure4")]
        public string StrMeasure4 { get; set; }
        [JsonPropertyName("strMeasure5")]
        public string StrMeasure5 { get; set; }
        [JsonPropertyName("strMeasure6")]
        public string StrMeasure6 { get; set; }
        [JsonPropertyName("strMeasure7")]
        public string StrMeasure7 { get; set; }
        [JsonPropertyName("strMeasure8")]
        public string StrMeasure8 { get; set; }
        [JsonPropertyName("strMeasure9")]
        public string StrMeasure9 { get; set; }
        [JsonPropertyName("strMeasure10")]
        public string StrMeasure10 { get; set; }
        [JsonPropertyName("strMeasure11")]
        public string StrMeasure11 { get; set; }
        [JsonPropertyName("strMeasure12")]
        public string StrMeasure12 { get; set; }
        [JsonPropertyName("strMeasure13")]
        public string StrMeasure13 { get; set; }
        [JsonPropertyName("strMeasure14")]
        public string StrMeasure14 { get; set; }
        [JsonPropertyName("strMeasure15")]
        public string StrMeasure15 { get; set; }
        [JsonPropertyName("strMeasure16")]
        public string StrMeasure16 { get; set; }
        [JsonPropertyName("strMeasure17")]
        public string StrMeasure17 { get; set; }
        [JsonPropertyName("strMeasure18")]
        public string StrMeasure18 { get; set; }
        [JsonPropertyName("strMeasure19")]
        public string StrMeasure19 { get; set; }
        [JsonPropertyName("strMeasure20")]
        public string StrMeasure20 { get; set; }

        public List<(string Ingredient, string Measure)> GetIngredientPairs()
        {
            var pairs = new List<(string Ingredient, string Measure)>();
            var type = GetType();

            for (int i = 1; i <= 20; i++)
            {
                var ingredient = type.GetProperty($"StrIngredient{i}")?.GetValue(this) as string;
                var measure = type.GetProperty($"StrMeasure{i}")?.GetValue(this) as string;

                if (!string.IsNullOrWhiteSpace(ingredient) && !string.IsNullOrWhiteSpace(measure))
                {
                    pairs.Add((ingredient.Trim(), measure.Trim()));
                }
            }

            return pairs;
        }
    }
}
