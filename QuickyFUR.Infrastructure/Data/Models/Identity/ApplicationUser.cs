using Microsoft.AspNetCore.Identity;
using QuickyFUR.Infrastructure.Constraints;
using QuickyFUR.Infrastructure.Messages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickyFUR.Infrastructure.Data.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(UserConstraints.NAME_MAX_LENGTH,
                      MinimumLength = UserConstraints.NAME_MIN_LENGTH,
                      ErrorMessage = ErrorMessages.stringLengthErrorMessage)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(UserConstraints.NAME_MAX_LENGTH,
              MinimumLength = UserConstraints.NAME_MIN_LENGTH,
              ErrorMessage = ErrorMessages.stringLengthErrorMessage)]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}"; 

/*        public Designer? Designer { get; set; }
        public Customer? Customer { get; set; }*/
    }
}
