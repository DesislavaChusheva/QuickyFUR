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
        public IList<ConfiguratedProduct> Products { get; set; } = new List<ConfiguratedProduct>();
    }
}
