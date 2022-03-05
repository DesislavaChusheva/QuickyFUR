using QuickyFUR.Constraints;
using QuickyFUR.Messages;
using System.ComponentModel.DataAnnotations;

namespace QuickyFUR.Data.Models
{
    public class Designer
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(DesignerConstraints.PSEUDONYM_MAX_LENGTH,
                      MinimumLength = DesignerConstraints.PSEUDONYM_MIN_LENGTH,
                      ErrorMessage = ErrorMessages.stringLengthErrorMessage)]
        public string Pseudonym { get; set; }

        [Required]
        [StringLength(64)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(DesignerConstraints.COUNTRY_MAX_LENGTH,
                      MinimumLength = DesignerConstraints.COUNTRY_MIN_LENGTH,
                      ErrorMessage = ErrorMessages.stringLengthErrorMessage)]
        public string Country { get; set; }

        [Required]
        [Range(DesignerConstraints.AGE_MIN, 
               DesignerConstraints.AGE_MAX, 
               ErrorMessage = ErrorMessages.ageErrorMessage)]
        public int Age { get; set; }

        [Required]
        [StringLength(DesignerConstraints.AUTOBIOGRAPHY_MAX_LENGTH,
                      ErrorMessage = ErrorMessages.noLongerThanErrorMessage)]
        public string Autobiography { get; set; }
        
        public ICollection<Field> Fields = new List<Field>(); 

        public ICollection<Product> Products = new List<Product>();
    }
}
