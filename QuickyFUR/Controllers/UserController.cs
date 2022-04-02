using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace QuickyFUR.Controllers
{
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(RoleManager<IdentityRole> _roleManager)
        {
            roleManager = _roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
/*        public async Task<ActionResult> CreateRole()
        {
            await roleManager.CreateAsync(new IdentityRole()
            {
                //Name = "Designer"
                Name = "Customer"
            });

            return Ok();
        }*/
    }
}
