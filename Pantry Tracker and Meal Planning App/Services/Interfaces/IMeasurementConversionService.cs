namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Services.Interfaces
{
    public interface IMeasurementConversionService
    {
        decimal Convert(decimal value, string fromUnit, string toUnit);
        bool CanConvert(string fromUnit, string toUnit);
        IEnumerable<string> GetSupportedUnits();
        string GetBaseUnit(string unit);
        decimal Convert(decimal? quantity, string unit, string v);
    }
}
