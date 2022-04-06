using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuickyFUR.Infrastructure.Constraints;
using QuickyFUR.Infrastructure.Data;
using QuickyFUR.Infrastructure.Data.Models;
using QuickyFUR.Infrastructure.Data.Models.Identity;
using QuickyFUR.Infrastructure.Data.Repositories;
using QuickyFUR.Infrastructure.Messages;
using System.ComponentModel.DataAnnotations;

namespace QuickyFUR.Areas.Identity.Pages.Account
{
    public class RegisterDesignerCompleteModel : PageModel
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IApplicationDbRepository _repo;

        public RegisterDesignerCompleteModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IApplicationDbRepository repo)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _repo = repo;
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


            await _repo.AddAsync(designer);
            await _repo.SaveChangesAsync();

            return Redirect("/");
        }
        private Designer CreateDesigner()
        {
            try
            {
                var userId = _userManager.GetUserId(User);
                ApplicationUser appUser = _repo.All<ApplicationUser>().FirstOrDefault(u => u.Id == userId);

                var designer = new Designer()
                {
                    ApplicationUser = appUser,
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

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
