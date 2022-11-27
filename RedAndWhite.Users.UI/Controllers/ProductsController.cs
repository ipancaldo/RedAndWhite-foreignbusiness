﻿using Microsoft.AspNetCore.Mvc;
using RedAndWhite.Service.Products;

namespace RedAndWhite.Users.UI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var test = _productService.GetAllProducts();
            return View();
        }
    }
}
