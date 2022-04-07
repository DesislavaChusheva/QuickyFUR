using QuickyFUR.Infrastructure.Constraints;
using QuickyFUR.Infrastructure.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickyFUR.Core.Models
{
    public class EditProductViewModel
    {
        [Required]
        [StringLength(ProductConstraints.NAME_MAX_LENGTH,
                      MinimumLength = ProductConstraints.NAME_MIN_LENGTH,
                      ErrorMessage = ErrorMessages.stringLengthErrorMessage)]
        public string? Name { get; set; }

        [Required]
        public string? Category { get; set; }

        [Required]
        public byte[]? Image { get; set; }


        [Required]
        [StringLength(ProductConstraints.DESCRIPTION_MAX_LENGTH,
                      ErrorMessage = ErrorMessages.noLongerThanErrorMessage)]
        public string? Descritpion { get; set; }

        [Required]
        public string? ConfiguratorLink { get; set; }
    }
}
