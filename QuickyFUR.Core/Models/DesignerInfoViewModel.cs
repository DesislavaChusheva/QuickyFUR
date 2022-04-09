using QuickyFUR.Infrastructure.Data.Models;
using QuickyFUR.Infrastructure.Data.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickyFUR.Core.Models
{
    public class DesignerInfoViewModel
    {
        public string FullName { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }
        public string Autobiography { get; set; }

        public IEnumerable<AllProductsByDesignerViewModel> Products = new List<AllProductsByDesignerViewModel>();
    }
}
