using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickyFUR.Core.Models
{
    public class OrderProductViewModel
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }

        public string? Category { get; set; }

        public string? ImageLink { get; set; }

        public string? DesignerName { get; set; }

        public string? Descritpion { get; set; }
        public string? ConfiguratorLink { get; set; }

        /*public string Dimensions { get; set; }

        public string Materials { get; set; }

        public decimal Price { get; set; }*/
    }
}
