using Microsoft.AspNetCore.Authorization;
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

        public async Task<IActionResult> CustomerAdditionalInformation()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);
            var customer = await _customerService.GetCustomerAsync(userId);
            return View(customer);
        }
        [HttpPost]
        public async Task<IActionResult> CustomerAdditionalPost(EditCustomerProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

            await _customerService.EditCustomerProfile(model, userId);
            return Redirect("/Identity/Account/Manage");
        }
    }
}
