using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickyFUR.Data.Models
{
    public class AppUser
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(64)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(36)]
        public string RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public AppRole Role { get; set; }

    }
}
