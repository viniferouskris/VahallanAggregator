using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vahallan_Ingredient_Aggregator.Controllers
{
    [Authorize] // Base authorization - must be logged in

    public class PantryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
