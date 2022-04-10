﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using QuickyFUR.Core.Contracts;
using QuickyFUR.Core.Models;
using QuickyFUR.Infrastructure.Data.Models.Identity;
using System.Text;

namespace QuickyFUR.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerService _customerService;

        public CustomerController(RoleManager<IdentityRole> roleManager,
                                  UserManager<ApplicationUser> userManager,
                                  ICustomerService customerService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _customerService = customerService;
        }
        [HttpGet]
        public IActionResult Categories()
        {
            var products = _customerService.ProductsByCategory("Tables");
            return View(products);
        }
        public async Task<IActionResult> OrderProduct(int productId)
        {
            var product = await _customerService.GetProductForOrder(productId);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart( int productId)
        {
            var file = Request.Form.Files[0];
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(reader.ReadLine());
            }
            string json = result.ToString().TrimEnd();

            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

            await _customerService.OrderProduct(json, productId, userId);
            return Redirect("/Identity/Customer/Cart");
        }
        public async Task<IActionResult> Cart(string cartId)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);
            var model = await _customerService.GetCart(userId);
            return View(model);
        }
        public async Task<IActionResult> MoreAboutDesigner(int productId)
        {
            var model = await _customerService.GetDesignerInfoForThisProduct(productId);
            return View(model);
        }

        public async Task<IActionResult> BuyProducts(string cartId)
        {
            await _customerService.BuyProductsFromCart(cartId);
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Categories(int categoryId)
        {
            var category = Request.Form["Category"].ToString();
            var products = _customerService.ProductsByCategory(category);
            return View(products);
        }
    }
}
