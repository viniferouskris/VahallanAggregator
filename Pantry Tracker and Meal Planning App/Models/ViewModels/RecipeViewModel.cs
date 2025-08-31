namespace Vahallan_Ingredient_Aggregator.Models.ViewModels
{
    public class RecipeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Collection { get; set; } = string.Empty;
        public string Description { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public int TotalTime { get; set; }
        public bool IsPublic { get; set; }
        public bool IsOwner { get; set; }
        public string CreatedBy { get; set; }
        public int IngredientsCount { get; set; }


        public string ExternalSource { get; set; }
        public string ExternalId { get; set; }
        public bool IsExternal => !string.IsNullOrEmpty(ExternalSource);

        public bool ShowInIngredientsList { get; set; } = false;
        public RecipeAccuracyLevel AccuracyLevel { get; set; } = RecipeAccuracyLevel.Estimate;
    }

}


