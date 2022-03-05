using QuickyFUR.Constraints;
using QuickyFUR.Messages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickyFUR.Data.Models
{
    public class Buyer
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(BuyerConstraints.NAME_MAX_LENGTH,
                      MinimumLength = BuyerConstraints.NAME_MIN_LENGTH,
                      ErrorMessage = ErrorMessages.stringLengthErrorMessage)]
        public string Name { get; set; }

        [Required]
        [StringLength(64)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(BuyerConstraints.ADDRESS_MAX_LENGTH,
                      MinimumLength = BuyerConstraints.ADDRESS_MIN_LENGTH,
                      ErrorMessage = ErrorMessages.stringLengthErrorMessage)]
        public string Address { get; set; }

        [Required]
        [StringLength(36)]
        public string CartId { get; set; }
        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; }
    }
}
