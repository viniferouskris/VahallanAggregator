using Microsoft.AspNetCore.Mvc;

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
