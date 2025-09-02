// Controllers/ProductionController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vahallan_Ingredient_Aggregator.Services.Interfaces;
using Vahallan_Ingredient_Aggregator.Models;

namespace Vahallan_Ingredient_Aggregator.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductionController : Controller
    {
        private readonly IProductionService _productionService;
        private readonly ILogger<ProductionController> _logger;

        public ProductionController(IProductionService productionService, ILogger<ProductionController> logger)
        {
            _productionService = productionService;
            _logger = logger;
        }

        // GET: Production/Upload
        public IActionResult Upload()
        {
            return View();
        }

        // POST: Production/ProcessCsv
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessCsv(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select a CSV file.");
                return View("Upload");
            }

            if (!csvFile.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                ModelState.AddModelError("", "Please upload a CSV file.");
                return View("Upload");
            }

            try
            {
                using var stream = csvFile.OpenReadStream();
                var report = await _productionService.ProcessProductionCsvAsync(stream);

                // Instead of TempData, pass directly to avoid 431 error
                return View("Report", report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing production CSV");
                ModelState.AddModelError("", $"Error processing CSV: {ex.Message}");
                return View("Upload");
            }
        }

        // GET: Production/Report - Remove this method since we pass directly now
        // public IActionResult Report() - REMOVED

        // GET: Production/Sample
        public IActionResult Sample()
        {
            // Generate a sample CSV file for download
            var csv = "Customer,Pattern,Colorway,Sq Ft,Sheets,Due Date,Notes,Lot\n";
            csv += "ABC Company,Eden,Standard,150.5,6.02,2025-02-15,Rush order,LOT001\n";
            csv += "XYZ Corp,Forest Bloom,Green,200.0,8.0,2025-02-20,Standard,LOT002\n";
            csv += "Design Studio,Eden,Silver,75.25,3.01,2025-02-10,Sample,LOT003\n";

            var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
            return File(bytes, "text/csv", "sample-production.csv");
        }
    }
}