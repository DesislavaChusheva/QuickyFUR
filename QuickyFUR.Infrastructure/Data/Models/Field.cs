using QuickyFUR.Infrastructure.Constraints;
using QuickyFUR.Infrastructure.Messages;
using System.ComponentModel.DataAnnotations;

namespace QuickyFUR.Infrastructure.Data.Models
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

        public IList<Product> Products = new List<Product>();

        public IList<ConfiguratedProduct> ConfiguratedProducts = new List<ConfiguratedProduct>();

        public IList<Designer> Designers = new List<Designer>();
    }
}
