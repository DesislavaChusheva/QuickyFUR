using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickyFUR.Core.Contracts;
using QuickyFUR.Core.Models;
using QuickyFUR.Infrastructure.Data.Models.Identity;

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

        public IActionResult Categories()
        {
            var products = _customerService.ProductsByCategory(1);
            return View(products);
        }
        public async Task<IActionResult> OrderProduct(int productId)
        {
            var product = await _customerService.GetProductForOrder(4);
            return View(product);
        }
        public IActionResult AddToCart()
        {
            /* var product = await _customerService.OrderProduct(4);
             return View(product);
             return View();*/
            return Redirect("/Identity/Customer/Cart");
        }
        public IActionResult Cart(string cartId)
        {
            var model = new List<ProductsInCartViewModel>();
/*            if (cartId == null)
            {
            model = _customerService.GetCart(cartId);
            }*/
            return View(model);
        }
        public async Task<IActionResult> MoreAboutDesigner()
        {
            var model = await _customerService.GetDesignerInfoForThisProduct(4);
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            /*var product = await _customerService.OrderProduct(4);*/

            
            return Redirect("/Identity/Customer/Cart");
        }
        [HttpPost]
        public async Task<IActionResult> Categories(IFormCollection form)
        {
            var result = form.Keys.First();
            /*int categoryId = int.Parse(result);*/
            var products = _customerService.ProductsByCategory(2);
            return View(products);
        }
    }
}
