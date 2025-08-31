namespace Vahallan_Ingredient_Aggregator.Models.Components
{
    
        public class RecipeIngredient
        {
            public int Id { get; set; }
            public int RecipeId { get; set; }
            public int IngredientId { get; set; }
            public decimal Quantity { get; set; }
            public string Unit { get; set; }


            public virtual Recipe Recipe { get; set; }
            public virtual Ingredient Ingredient { get; set; }
        }

    
}
