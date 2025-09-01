using Microsoft.AspNetCore.Mvc;
using Vahallan_Ingredient_Aggregator.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace Vahallan_Ingredient_Aggregator.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IWebHostEnvironment _env;
        public HomeController(
            ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("/dev-login")]
        public async Task<IActionResult> DevLogin()
        {
            if (!_env.IsDevelopment())
            {
                return NotFound(); // Only works in development
            }

            var user = await _userManager.FindByEmailAsync("admin@yourapp.com");
            if (user != null)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return RedirectToAction("Index", "Recipe");
        }
    }
}
