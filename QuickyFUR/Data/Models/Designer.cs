using QuickyFUR.Constraints;
using QuickyFUR.Messages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickyFUR.Data.Models
{
    public class Designer
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(36)]
        public string AppUserId { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public AppUser AppUser { get; set; }

        [StringLength(DesignerConstraints.COUNTRY_MAX_LENGTH,
                      MinimumLength = DesignerConstraints.COUNTRY_MIN_LENGTH,
                      ErrorMessage = ErrorMessages.stringLengthErrorMessage)]
        public string? Country { get; set; }

        [Range(DesignerConstraints.AGE_MIN, 
               DesignerConstraints.AGE_MAX, 
               ErrorMessage = ErrorMessages.ageErrorMessage)]
        public int Age { get; set; }

        [StringLength(DesignerConstraints.AUTOBIOGRAPHY_MAX_LENGTH,
                      ErrorMessage = ErrorMessages.noLongerThanErrorMessage)]
        public string? Autobiography { get; set; }
        
        public IList<Field> Fields = new List<Field>(); 

        public IList<Product> Products = new List<Product>();
    }
}
