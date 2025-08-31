using Vahallan_Ingredient_Aggregator.Services.Interfaces;

namespace Vahallan_Ingredient_Aggregator.Services.Implementaions
{
    // Services/Implementations/MeasurementConversionService.cs
    public class MeasurementConversionService : IMeasurementConversionService
    {
        private readonly Dictionary<string, decimal> _conversionFactors;

        public MeasurementConversionService()
        {
            _conversionFactors = new Dictionary<string, decimal>
        {
            {"g", 1},
            {"kg", 1000},
            {"oz", 28.3495m},
            {"lb", 453.592m},
            {"ml", 1},
            {"l", 1000},
            {"cup", 236.588m},
            {"tbsp", 14.7868m},
            {"tsp", 4.92892m}
        };
        }

        public decimal Convert(decimal value, string fromUnit, string toUnit)
        {
            if (fromUnit == toUnit) return value;

            if (!_conversionFactors.ContainsKey(fromUnit) || !_conversionFactors.ContainsKey(toUnit))
                throw new ArgumentException("Unsupported unit");

            var baseValue = value * _conversionFactors[fromUnit];
            return baseValue / _conversionFactors[toUnit];
        }

        public bool CanConvert(string fromUnit, string toUnit)
        {
            return _conversionFactors.ContainsKey(fromUnit) && _conversionFactors.ContainsKey(toUnit);
        }

        public IEnumerable<string> GetSupportedUnits()
        {
            return _conversionFactors.Keys;
        }

        public string GetBaseUnit(string unit)
        {
            if (!_conversionFactors.ContainsKey(unit))
                throw new ArgumentException("Unsupported unit");

            // Return g for weight units, ml for volume units
            return unit is "g" or "kg" or "oz" or "lb" ? "g" : "ml";
        }

        public decimal Convert(decimal? quantity, string unit, string v)
        {
            throw new NotImplementedException();
        }
    }

}
