using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickyFUR.Core.Models
{
    public class OrderProductViewModel
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public byte[] Image { get; set; }

        public string Designer { get; set; }

        public string Descritpion { get; set; }

        /*public string Dimensions { get; set; }

        public string Materials { get; set; }

        public decimal Price { get; set; }*/
    }
}
