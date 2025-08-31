using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pantry_Tracker_and_Meal_Planning_with_TheMealAPI_App.Controllers
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
