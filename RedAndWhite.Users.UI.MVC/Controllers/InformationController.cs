using Microsoft.AspNetCore.Mvc;
using RedAndWhite.Model.Images;
using RedAndWhite.Service.Informations;

namespace RedAndWhite.Users.UI.MVC.Controllers
{
    public class InformationController : Controller
    {
        private readonly IInformationService _informationService;

        public InformationController(IInformationService informationService)
        {
            _informationService = informationService;
        }

        public IActionResult Index()
        {
            try
            {
                _informationService.Create(new Model.Informations.NewInformationModel
                {
                    Title = "Test",
                    Text = "Test",
                    Images = new List<ImageModel> { new ImageModel { Image = "D:/Repos/RedAndWhite-foreignbusiness/RedAndWhite.Users.UI.MVC/wwwroot/images" } }
                });
                return View(_informationService.GetLastInformation());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
