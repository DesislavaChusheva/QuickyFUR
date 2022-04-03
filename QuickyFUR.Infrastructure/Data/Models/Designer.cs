using QuickyFUR.Infrastructure.Constraints;
using QuickyFUR.Infrastructure.Data.Models.Identity;
using QuickyFUR.Infrastructure.Messages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickyFUR.Infrastructure.Data.Models
{
    public class Designer
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
       //[ForeignKey(nameof(ApplicationUser))]
        public ApplicationUser? ApplicationUser { get; set; }
       // public string? ApplicationUserId { get; set; }

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
        
        public IList<Field> Fields = new List<Field>(); 

        public IList<Product> Products = new List<Product>();
    }
}
