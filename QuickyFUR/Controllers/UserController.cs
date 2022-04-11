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
        /*public async Task<ActionResult> CreateRole()
        {
            await _roleManager.CreateAsync(new IdentityRole()
            {
                //Name = "Designer"
                Name = "Customer"
            });

            return Ok();
        }*/

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
