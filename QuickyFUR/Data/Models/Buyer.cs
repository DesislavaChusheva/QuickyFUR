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
        [StringLength(36)]
        public string AppUserId { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public AppUser AppUser { get; set; }

        [StringLength(BuyerConstraints.ADDRESS_MAX_LENGTH,
                      MinimumLength = BuyerConstraints.ADDRESS_MIN_LENGTH,
                      ErrorMessage = ErrorMessages.stringLengthErrorMessage)]
        public string? Address { get; set; }

        [StringLength(36)]
        public string? CartId { get; set; }
        [ForeignKey(nameof(CartId))]
        public Cart? Cart { get; set; }
    }
}
