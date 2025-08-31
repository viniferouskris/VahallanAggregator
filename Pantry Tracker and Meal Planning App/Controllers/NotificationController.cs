using Microsoft.AspNetCore.Mvc;

namespace Vahallan_Ingredient_Aggregator.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
