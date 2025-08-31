using Microsoft.AspNetCore.Mvc;

namespace Vahallan_Ingredient_Aggregator.Controllers
{
    public class ShoppingListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
