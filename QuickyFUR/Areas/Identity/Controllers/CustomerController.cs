using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuickyFUR.Core.Contracts;
using QuickyFUR.Infrastructure.Data.Models.Identity;

namespace QuickyFUR.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize(Roles = "Customer")]
    public class CustomerController
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
    }
}
