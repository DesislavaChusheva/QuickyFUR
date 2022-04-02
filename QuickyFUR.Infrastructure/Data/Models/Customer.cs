using QuickyFUR.Infrastructure.Constraints;
using QuickyFUR.Infrastructure.Data.Models.Identity;
using QuickyFUR.Infrastructure.Messages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickyFUR.Infrastructure.Data.Models
{
    public class Customer
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
       // [ForeignKey(nameof(ApplicationUser))]
        public ApplicationUser? ApplicationUser { get; set; }
        // public string? ApplicationUserId { get; set; }

        [Required]
        [StringLength(CustomerConstraints.ADDRESS_MAX_LENGTH,
                      MinimumLength = CustomerConstraints.ADDRESS_MIN_LENGTH,
                      ErrorMessage = ErrorMessages.stringLengthErrorMessage)]
        public string Address { get; set; }

        [StringLength(36)]
        public string? CartId { get; set; }
        [ForeignKey(nameof(CartId))]
        public Cart? Cart { get; set; }
    }
}
