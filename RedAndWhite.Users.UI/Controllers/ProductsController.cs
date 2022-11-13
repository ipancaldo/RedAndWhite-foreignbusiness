using Microsoft.AspNetCore.Mvc;

namespace RedAndWhite.Users.UI.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
