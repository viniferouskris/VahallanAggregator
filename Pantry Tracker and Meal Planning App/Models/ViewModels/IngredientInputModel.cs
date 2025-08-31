namespace Vahallan_Ingredient_Aggregator.Models.ViewModels
{
    public class IngredientInputModel
    {
        public int IngredientId { get; set; }
        public decimal Quantity { get; set; } = 0;
        public string Unit { get; set; }
    }
}
