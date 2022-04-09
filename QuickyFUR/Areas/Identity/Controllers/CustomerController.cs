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

        public async Task<IActionResult> AddToCart()
        {
            string json = @"{
  ""Dimensions"": ""600*1400*750"",
  ""Additions"": ""Fillet/Round"",
  ""Materials"": ""Black Stainless Steel/Wood"",
  ""Price"": ""594""
}";
            var product = await _customerService.OrderProduct(json, 4, "20f7d9a5-0e6a-41d4-9586-fa530130f903");
            return Redirect("/Identity/Customer/Cart");
        }
        public async Task<IActionResult> Cart(string cartId)
        {
            var model = await _customerService.GetCart("77716156-7c3a-4e3e-b0dd-89ab2647b1e1");
            return View(model);
        }
        public async Task<IActionResult> MoreAboutDesigner()
        {
            var model = await _customerService.GetDesignerInfoForThisProduct(4);
            return View(model);
        }

/*        [HttpPost]
        public async Task<IActionResult> Cart()
        {

        }*/

        public async Task<IActionResult> BuyProducts()
        {
            await _customerService.BuyProductsFromCart("77716156-7c3a-4e3e-b0dd-89ab2647b1e1");
            return View();

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
