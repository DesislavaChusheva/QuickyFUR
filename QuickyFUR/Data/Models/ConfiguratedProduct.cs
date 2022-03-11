using QuickyFUR.Constraints;
using QuickyFUR.Messages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickyFUR.Data.Models
{
    public class ConfiguratedProduct
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(ProductConstraints.NAME_MAX_LENGTH,
                      MinimumLength = ProductConstraints.NAME_MIN_LENGTH,
                      ErrorMessage = ErrorMessages.stringLengthErrorMessage)]
        public string Name { get; set; }

        [Required]
        public int FieldId { get; set; }
        [ForeignKey(nameof(FieldId))]
        public Field Field { get; set; }

        [Required]
        public byte[] Image { get; set; }

        [Required]
        [StringLength(36)]
        public string DesignerId { get; set; }
        [ForeignKey(nameof(DesignerId))]
        public Designer Designer { get; set; }

        [Required]
        [StringLength(ProductConstraints.DESCRIPTION_MAX_LENGTH,
                      ErrorMessage = ErrorMessages.noLongerThanErrorMessage)]
        public string Descritpion { get; set; }

        [Required]
        public string Dimensions { get; set; }

        [Required]
        public string Materials { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [StringLength(36)]
        public string CartId { get; set; }
        [ForeignKey(nameof(CartId))]
        public Cart Cart { get; set; }
    }
}
