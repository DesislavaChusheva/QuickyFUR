using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickyFUR.Infrastructure.Data.Models
{
    public class Cart
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public Customer Customer { get; set; }

        public IList<ConfiguratedProduct> Products { get; set; } = new List<ConfiguratedProduct>();
    }
}
