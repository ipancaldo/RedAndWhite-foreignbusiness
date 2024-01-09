using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    // GET: Error
    public ActionResult Index(Exception ex)
    {
        // Log the exception or perform any other error handling logic here

        // Pass the exception details to the view
        ViewBag.Exception = ex;

        return View();
    }
}