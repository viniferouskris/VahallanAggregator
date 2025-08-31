namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Models.ViewModels
{
    public class IngredientInputModel
    {
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; } = 0;
        public string Unit { get; set; }
    }
}
