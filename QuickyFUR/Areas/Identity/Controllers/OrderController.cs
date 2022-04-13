using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickyFUR.Core.Contracts;
using QuickyFUR.Infrastructure.Data.Models.Identity;

namespace QuickyFUR.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDesignerService _designerService;
        private readonly ICustomerService _customerService;

        public OrderController(RoleManager<IdentityRole> roleManager,
                                  UserManager<ApplicationUser> userManager,
                                  IDesignerService designerService,
                                  ICustomerService customerService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _designerService = designerService;
            _customerService = customerService;
        }

        [Authorize(Roles = "Designer")]
        public async Task<IActionResult> Orders()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

            var orders = await _designerService.GetOrdersAsync(userId);

            return View(orders);
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

            var orders = await _customerService.GetOrders(userId);

            return View(orders);
        }
    }
}
