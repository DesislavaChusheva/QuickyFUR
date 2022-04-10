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
    public class EditDesignerProfileViewModel
    {

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
    }
}
