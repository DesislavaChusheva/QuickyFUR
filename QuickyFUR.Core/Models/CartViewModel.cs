using QuickyFUR.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickyFUR.Core.Models
{
    public class CartViewModel
    {
        public IList<ProductsInCartViewModel> Products { get; set; } = new List<ProductsInCartViewModel>();
        public decimal TotalPrice { get; set; }
    }
}
