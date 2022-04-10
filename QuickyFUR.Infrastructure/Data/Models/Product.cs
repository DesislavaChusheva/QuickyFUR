using QuickyFUR.Infrastructure.Constraints;
using QuickyFUR.Infrastructure.Messages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickyFUR.Infrastructure.Data.Models
{
    public class Product
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ProductConstraints.NAME_MAX_LENGTH,
                      MinimumLength = ProductConstraints.NAME_MIN_LENGTH,
                      ErrorMessage = ErrorMessages.stringLengthErrorMessage)]
        public string? Name { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        [Required]
        public string? ImageLink { get; set; }

        [Required]
        [StringLength(36)]
        public string? DesignerId { get; set; }
        [ForeignKey(nameof (DesignerId))]
        public Designer? Designer { get; set; }

        [Required]
        [StringLength(ProductConstraints.DESCRIPTION_MAX_LENGTH,
                      ErrorMessage = ErrorMessages.noLongerThanErrorMessage)]
        public string? Descritpion { get; set; }

        [Required]
        public string? ConfiguratorLink { get; set; }

        public bool Deleted { get; set; } = false;
    }
}
