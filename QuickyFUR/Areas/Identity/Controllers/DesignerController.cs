﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickyFUR.Core.Contracts;
using QuickyFUR.Core.Models;
using QuickyFUR.Infrastructure.Data.Models;
using QuickyFUR.Infrastructure.Data.Models.Identity;

namespace QuickyFUR.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize(Roles = "Designer")]
    public class DesignerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDesignerService _designerService;

        public DesignerController(RoleManager<IdentityRole> roleManager,
                                  UserManager<ApplicationUser> userManager,
                                  IDesignerService designerService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _designerService = designerService;
        }

        public IActionResult CreateProduct()
        {
            return View();
        }
        public async Task<IActionResult> EditProduct(int productId)
        {
            var product = await _designerService.GetProductForEditAsync(productId);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProductPost(EditProductViewModel model, int productId)
        {
            var product = await _designerService.EditProductAsync(model, productId);
            return Redirect("/Identity/Designer/MyProducts");
        }
        public IActionResult DeleteProduct(int productId)
        {
            return View();
        }
        /*
                [HttpPost]
                public async Task<IActionResult> EditProduct(int productId)
                {
                    var 
                }
        */
        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

            await _designerService.AddProductAsync(model, userId);
            return Redirect("/Identity/Designer/MyProducts");
        }

        public IActionResult MyProducts()
        {
            IEnumerable<AllProductsViewModel> products = _designerService.MyProducts(_userManager.GetUserId(User));
            return View(products);
        }

        /*public async Task<ActionResult> CreateCategories()
        {
            Category[] categories = new Category[]
            {
                new Category() { Name = "Tables"},
                new Category() { Name = "Chairs"},
                new Category() { Name = "Armchairs"},
                new Category() { Name = "Sofas"},
                new Category() { Name = "Benches"},
                new Category() { Name = "Wardrobes"},
                new Category() { Name = "KitchenCabinets"},
                new Category() { Name = "Beds"},
                new Category() { Name = "NightStands"},
            };
            await _data.AddRangeAsync();
            await _data.SaveChangesAsync();

            return Ok();
        }*/

    }
}
