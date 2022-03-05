using QuickyFUR.Constraints;
using QuickyFUR.Messages;
using System.ComponentModel.DataAnnotations;

namespace QuickyFUR.Data.Models
{
    public class Field
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(FieldConstraints.NAME_MAX_LENGTH,
                      MinimumLength = FieldConstraints.NAME_MIN_LENGTH,
                      ErrorMessage = ErrorMessages.stringLengthErrorMessage)]
        public string Name { get; set; }

        public ICollection<Product> Products = new List<Product>();

        public ICollection<Designer> Designers = new List<Designer>();
    }
}
