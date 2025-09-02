using Vahallan_Ingredient_Aggregator.Models.Components;
using Vahallan_Ingredient_Aggregator.Models;

namespace Vahallan_Ingredient_Aggregator.Services.Interfaces
{
    public interface IProductionService
    {
        Task<ProductionReport> ProcessProductionCsvAsync(Stream csvStream);
        Task<ProductionReport> GenerateProductionReportAsync(List<ProductionOrder> orders);
    }
}
