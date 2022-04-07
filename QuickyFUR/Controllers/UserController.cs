using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace QuickyFUR.Controllers
{
    public class UserController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
/*        public async Task<ActionResult> CreateRole()
        {
            await _roleManager.CreateAsync(new IdentityRole()
            {
                //Name = "Designer"
                Name = "Customer"
            });

            return Ok();
        }*/



    }
}
