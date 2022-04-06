using QuickyFUR.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickyFUR.Core.Models
{
    public class AllProductsViewModel
    {
        public string Name { get; set; }

        public string Field { get; set; }

        public byte[] Image { get; set; }

        public string DesignerId { get; set; }
        [ForeignKey(nameof(DesignerId))]
        public string Designer { get; set; }

        public string Descritpion { get; set; }


        public string ConfiguratorLink { get; set; }
    }
}
