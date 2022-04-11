using Microsoft.AspNetCore.Authorization;
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

        public async Task<IActionResult> DesignerAdditionalInformation()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);
            var designer = await _designerService.GetDesignerAsync(userId);
            return View(designer);
        }

        [HttpPost]
        public async Task<IActionResult> DesignerAdditionalPost(EditDesignerProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

            await _designerService.EditDesignerProfile(model, userId);
            return Redirect("/Identity/Account/Manage");
        }
    }
}
