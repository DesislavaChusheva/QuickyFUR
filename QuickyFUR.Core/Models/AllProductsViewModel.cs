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
        public int ProdcuctId { get; set; }
        public string? Name { get; set; }

        public string? Category { get; set; }

        public string? ImageLink { get; set; }

/*        public string DesignerId { get; set; }
        [ForeignKey(nameof(DesignerId))]
        public string Designer { get; set; }*/
        public string? DesignerName { get; set; }

        public string? Descritpion { get; set; }

        public string? ConfiguratorLink { get; set; }
    }
}
