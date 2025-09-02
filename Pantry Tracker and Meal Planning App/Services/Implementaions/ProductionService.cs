// Services/Implementations/ProductionService.cs
using Microsoft.EntityFrameworkCore;
using Vahallan_Ingredient_Aggregator.Data;
using Vahallan_Ingredient_Aggregator.Models;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using UnitsNet;
using CsvHelper.Configuration.Attributes;
using Vahallan_Ingredient_Aggregator.Models.Components;

namespace Vahallan_Ingredient_Aggregator.Services.Implementations
{
    public class ProductionService : IProductionService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductionService> _logger;

        public ProductionService(ApplicationDbContext context, ILogger<ProductionService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ProductionReport> ProcessProductionCsvAsync(Stream csvStream)
        {
            var orders = new List<ProductionOrder>();

            try
            {
                using var reader = new StreamReader(csvStream);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                // Configure CSV reader to be very flexible
                csv.Context.Configuration.HeaderValidated = null;
                csv.Context.Configuration.MissingFieldFound = null;
                csv.Context.Configuration.BadDataFound = null;
                csv.Context.Configuration.PrepareHeaderForMatch = args => args.Header.Trim().Replace("\n", "").Replace("\r", "");

                // Read as dynamic records to handle messy headers
                var records = csv.GetRecords<dynamic>().ToList();
                _logger.LogInformation($"Read {records.Count} records from CSV");

                foreach (var record in records)
                {
                    var recordDict = record as IDictionary<string, object>;
                    if (recordDict == null) continue;

                    try
                    {
                        // Find the values by looking for headers that contain our keywords
                        var customer = FindValueByHeaderKeyword(recordDict, "customer");
                        var pattern = FindValueByHeaderKeyword(recordDict, "pattern");
                        var colorway = FindValueByHeaderKeyword(recordDict, "colorway");
                        var sqFtValue = FindValueByHeaderKeyword(recordDict, "sq ft");
                        var sheetsValue = FindValueByHeaderKeyword(recordDict, "sheets");
                        var dueDate = FindValueByHeaderKeyword(recordDict, "due date");
                        var notes = FindValueByHeaderKeyword(recordDict, "notes");
                        var lot = FindValueByHeaderKeyword(recordDict, "lot");

                        // Parse square feet
                        if (!decimal.TryParse(sqFtValue?.ToString(), out decimal sqFt) || sqFt <= 0)
                            continue;

                        // Parse sheets (optional)
                        decimal.TryParse(sheetsValue?.ToString(), out decimal sheets);

                        // Skip if missing essential data
                        if (string.IsNullOrWhiteSpace(customer) || string.IsNullOrWhiteSpace(pattern))
                            continue;

                        var order = new ProductionOrder
                        {
                            Customer = customer.Trim(),
                            Pattern = pattern.Trim(),
                            Colorway = colorway?.Trim() ?? "",
                            SquareFeet = sqFt,
                            Sheets = sheets,
                            DueDate = dueDate?.Trim() ?? "",
                            Notes = notes?.Trim() ?? "",
                            Lot = lot?.Trim() ?? ""
                        };

                        orders.Add(order);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Skipping problematic CSV row");
                        continue;
                    }
                }

                _logger.LogInformation($"Processed {orders.Count} valid orders");
                return await GenerateProductionReportAsync(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing production CSV");
                throw;
            }
        }

        private string FindValueByHeaderKeyword(IDictionary<string, object> record, string keyword)
        {
            try
            {
                // Find header that contains the keyword (case-insensitive, ignoring whitespace and newlines)
                var matchingKey = record.Keys.FirstOrDefault(key =>
                {
                    var cleanKey = key?.Replace("\n", "").Replace("\r", "").Trim() ?? "";
                    return cleanKey.Contains(keyword, StringComparison.OrdinalIgnoreCase);
                });

                if (matchingKey != null)
                {
                    var value = record[matchingKey]?.ToString();
                    _logger.LogDebug($"Found {keyword}: '{value}' using header '{matchingKey}'");
                    return value;
                }

                _logger.LogDebug($"Could not find column for keyword '{keyword}'. Available headers: {string.Join(", ", record.Keys)}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Error finding value for keyword '{keyword}'");
                return null;
            }
        }

        public async Task<ProductionReport> GenerateProductionReportAsync(List<ProductionOrder> orders)
        {
            var report = new ProductionReport
            {
                GeneratedAt = DateTime.UtcNow,
                TotalOrders = orders.Count,
                TotalSquareFeet = orders.Sum(o => o.SquareFeet)
            };

            // Get all recipes from database for pattern matching
            var recipes = await _context.Recipes
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .ToListAsync();

            var materialSummary = new Dictionary<string, MaterialSummary>();

            foreach (var order in orders)
            {
                // Try to find matching recipe (case-insensitive)
                var matchingRecipe = recipes.FirstOrDefault(r =>
                    string.Equals(r.Name, order.Pattern, StringComparison.OrdinalIgnoreCase));

                if (matchingRecipe != null)
                {
                    order.PatternExists = true;
                    order.RecipeId = matchingRecipe.Id;
                    report.MatchedPatterns++;

                    // Calculate material needs based on square footage
                    var scaleFactor = order.SquareFeet / matchingRecipe.StandardSheetSize;

                    foreach (var recipeIngredient in matchingRecipe.RecipeIngredients)
                    {
                        if (recipeIngredient.Ingredient == null) continue;

                        var scaledQuantity = recipeIngredient.Quantity * scaleFactor;

                        var materialNeed = new MaterialNeed
                        {
                            MaterialName = recipeIngredient.Ingredient.Name,
                            QuantityNeeded = scaledQuantity,
                            Unit = recipeIngredient.Unit,
                            MaterialType = recipeIngredient.Ingredient.MaterialType,
                            EstimatedCost = CalculateEstimatedCost(recipeIngredient.Ingredient, scaledQuantity, recipeIngredient.Unit)
                        };

                        order.MaterialNeeds.Add(materialNeed);

                        // Add to summary
                        AddToMaterialSummary(materialSummary, materialNeed, recipeIngredient.Ingredient);
                    }
                }
                else
                {
                    order.PatternExists = false;
                    report.UnknownPatternsCount++; // Fixed: use Count property

                    if (!report.UnknownPatternsList.Contains(order.Pattern))
                    {
                        report.UnknownPatternsList.Add(order.Pattern);
                    }
                }
            }

            report.Orders = orders;
            report.MaterialSummary = materialSummary.Values.ToList();

            return report;
        }

        private decimal CalculateEstimatedCost(Ingredient ingredient, decimal quantity, string unit)
        {
            try
            {
                // Convert the needed quantity to the ingredient's stored unit
                var convertedQuantity = ConvertQuantity(quantity, unit, ingredient.Unit);
                var unitsNeeded = convertedQuantity / ingredient.UnitsPerPackage;
                return Math.Ceiling(unitsNeeded) * ingredient.CostPerPackage;
            }
            catch
            {
                return 0;
            }
        }

        private decimal ConvertQuantity(decimal quantity, string fromUnit, string toUnit)
        {
            if (string.Equals(fromUnit, toUnit, StringComparison.OrdinalIgnoreCase))
                return quantity;

            try
            {
                // Use UnitsNet for conversion
                if (TryConvertVolume((double)quantity, fromUnit, toUnit, out double convertedVolume))
                    return (decimal)convertedVolume;

                if (TryConvertMass((double)quantity, fromUnit, toUnit, out double convertedMass))
                    return (decimal)convertedMass;
            }
            catch { }

            return quantity; // Fallback to original if conversion fails
        }

        private bool TryConvertVolume(double quantity, string fromUnit, string toUnit, out double result)
        {
            result = 0;
            try
            {
                var volume = ParseVolumeUnit(quantity, fromUnit);
                if (volume.HasValue)
                {
                    result = ConvertVolumeToUnit(volume.Value, toUnit);
                    return true;
                }
            }
            catch { }
            return false;
        }

        private bool TryConvertMass(double quantity, string fromUnit, string toUnit, out double result)
        {
            result = 0;
            try
            {
                var mass = ParseMassUnit(quantity, fromUnit);
                if (mass.HasValue)
                {
                    result = ConvertMassToUnit(mass.Value, toUnit);
                    return true;
                }
            }
            catch { }
            return false;
        }

        private Volume? ParseVolumeUnit(double quantity, string unit)
        {
            return unit?.ToLower().Trim() switch
            {
                "gallons" or "gallon" or "gal" => Volume.FromUsGallons(quantity),
                "quarts" or "quart" or "qt" => Volume.FromUsQuarts(quantity),
                "cups" or "cup" => Volume.FromLiters(quantity * 0.236588),
                "fl oz" or "ounces" or "fluid ounces" => Volume.FromLiters(quantity * 0.0295735),
                "liters" or "liter" or "l" => Volume.FromLiters(quantity),
                _ => null
            };
        }

        private Mass? ParseMassUnit(double quantity, string unit)
        {
            return unit?.ToLower().Trim() switch
            {
                "lbs" or "pounds" or "pound" or "lb" => Mass.FromPounds(quantity),
                "oz" or "ounces" or "ounce" => Mass.FromOunces(quantity),
                "kg" or "kilograms" or "kilogram" => Mass.FromKilograms(quantity),
                "g" or "grams" or "gram" => Mass.FromGrams(quantity),
                _ => null
            };
        }

        private double ConvertVolumeToUnit(Volume volume, string toUnit)
        {
            return toUnit?.ToLower().Trim() switch
            {
                "gallons" or "gallon" or "gal" => volume.UsGallons,
                "quarts" or "quart" or "qt" => volume.UsQuarts,
                "cups" or "cup" => volume.Liters / 0.236588,
                "fl oz" or "ounces" or "fluid ounces" => volume.Liters / 0.0295735,
                "liters" or "liter" or "l" => volume.Liters,
                _ => volume.Liters
            };
        }

        private double ConvertMassToUnit(Mass mass, string toUnit)
        {
            return toUnit?.ToLower().Trim() switch
            {
                "lbs" or "pounds" or "pound" or "lb" => mass.Pounds,
                "oz" or "ounces" or "ounce" => mass.Ounces,
                "kg" or "kilograms" or "kilogram" => mass.Kilograms,
                "g" or "grams" or "gram" => mass.Grams,
                _ => mass.Pounds
            };
        }

        private void AddToMaterialSummary(Dictionary<string, MaterialSummary> summary, MaterialNeed need, Ingredient ingredient)
        {
            if (!summary.ContainsKey(need.MaterialName))
            {
                summary[need.MaterialName] = new MaterialSummary
                {
                    MaterialName = need.MaterialName,
                    Unit = need.Unit,
                    MaterialType = need.MaterialType,
                    CurrentInventory = ingredient.Quantity
                };
            }

            var existing = summary[need.MaterialName];

            // Convert quantities to same unit for proper aggregation
            var convertedQuantity = ConvertQuantity(need.QuantityNeeded, need.Unit, existing.Unit);

            existing.TotalQuantityNeeded += convertedQuantity;
            existing.TotalEstimatedCost += need.EstimatedCost;
            existing.ShortfallQuantity = Math.Max(0, existing.TotalQuantityNeeded - existing.CurrentInventory);
        }
    }
}