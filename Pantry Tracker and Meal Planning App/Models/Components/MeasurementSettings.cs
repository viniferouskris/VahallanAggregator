namespace Vahallan_Ingredient_Aggregator.Models.Components
{
    public class MeasurementSettings
    {
        public string DefaultSystem { get; set; } = "Metric";
        public bool EnableAutomaticConversion { get; set; } = true;
    }
}
