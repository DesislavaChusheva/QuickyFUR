using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickyFUR.Core.Models
{
    public class OrderPageViewModel
    {
        public string? DorCSide { get; set; }
        public IList<OrderedProductsViewModel> Products { get; set; } = new List<OrderedProductsViewModel>();
    }
}
