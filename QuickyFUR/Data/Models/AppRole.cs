using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace QuickyFUR.Data.Models
{
    public class AppRole
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        public IList<AppUser> AppUsers { get; set; } = new List<AppUser>();

    }
}
