using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuickyFUR.Infrastructure.Constraints;
using QuickyFUR.Infrastructure.Data;
using QuickyFUR.Infrastructure.Data.Models;
using QuickyFUR.Infrastructure.Data.Models.Identity;
using QuickyFUR.Infrastructure.Messages;
using System.ComponentModel.DataAnnotations;

namespace QuickyFUR.Areas.Identity.Pages.Account
{
    public class RegisterDesignerCompleteModel : PageModel
    {

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _data;

        public RegisterDesignerCompleteModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext data
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _data = data;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Required]
            public string Country { get; set; }

            [Required]
            [Range(DesignerConstraints.AGE_MIN,
                   DesignerConstraints.AGE_MAX,
                   ErrorMessage = ErrorMessages.ageErrorMessage)]

            public int Age { get; set; }

            [StringLength(DesignerConstraints.AUTOBIOGRAPHY_MAX_LENGTH,
                          ErrorMessage = ErrorMessages.noLongerThanErrorMessage)]

            [Required]
            public string Autobiography { get; set; }
        }
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            Designer designer = CreateDesigner();


            await _data.AddAsync(designer);
            await _data.SaveChangesAsync();

            return Redirect("/");
        }
        private Designer CreateDesigner()
        {
            try
            {
                var designer = new Designer()
                {
                    ApplicationUser = (ApplicationUser)_userManager.Users.First(),
                    Country = Input.Country,
                    Age = Input.Age,
                    Autobiography = Input.Autobiography
                };
                return designer;
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
