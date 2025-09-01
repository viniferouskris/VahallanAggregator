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

        //   public string Notes { get; set; } // "Apply with sponge texture"

        //public decimal PurchaseQuantityNeeded
        //{
        //    get
        //    {
        //        // This would use a conversion service to convert working units to purchase units
        //        // e.g., 2 cups -> 0.125 gallons
        //        return ConvertToPurchaseUnits(Quantity, Unit, Ingredient.Unit);
        //    }
        //}

        public bool Validate(out List<string> errors)
        {
            errors = new List<string>();

            if (Quantity <= 0)
                errors.Add("Quantity must be greater than zero");
            if (string.IsNullOrWhiteSpace(Unit))
                errors.Add("Unit is required");

            return errors.Count == 0;
        }
    }

    
}
