using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickyFUR.Data.Models
{
    public class Cart
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public Buyer Buyer { get; set; }

        public ICollection<ConfiguratedProduct> Products { get; set; } = new List<ConfiguratedProduct>();
    }
}
