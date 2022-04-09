using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using QuickyFUR.Infrastructure.Constraints;
using QuickyFUR.Infrastructure.Data.Models;
using QuickyFUR.Infrastructure.Messages;
using System.ComponentModel.DataAnnotations;

namespace QuickyFUR.Core.Models
{
    public class CreateProductViewModel
    {

        [Required]
        [StringLength(ProductConstraints.NAME_MAX_LENGTH,
                      MinimumLength = ProductConstraints.NAME_MIN_LENGTH,
                      ErrorMessage = ErrorMessages.stringLengthErrorMessage)]
        public string? Name { get; set; }

        [Required]
        public string? Category { get; set; }

        [Required]
        public string? ImageLink { get; set; }


        [Required]
        [StringLength(ProductConstraints.DESCRIPTION_MAX_LENGTH,
                      ErrorMessage = ErrorMessages.noLongerThanErrorMessage)]
        public string? Descritpion { get; set; }

        [Required]
        public string? ConfiguratorLink { get; set; }

    }
}
