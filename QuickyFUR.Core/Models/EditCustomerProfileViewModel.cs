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
    public class EditCustomerProfileViewModel
    {
        [Required]
        [StringLength(CustomerConstraints.ADDRESS_MAX_LENGTH,
                         MinimumLength = CustomerConstraints.ADDRESS_MIN_LENGTH,
                         ErrorMessage = ErrorMessages.stringLengthErrorMessage)]
        public string Address { get; set; }
    }
}
