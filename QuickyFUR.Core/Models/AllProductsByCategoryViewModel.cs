using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickyFUR.Core.Models
{
    public class AllProductsByCategoryViewModel
    {
        public int ProdcuctId { get; set; }
        public string? Name { get; set; }

        public string? ImageLink { get; set; }

        /*        public string DesignerId { get; set; }
                [ForeignKey(nameof(DesignerId))]
                public string Designer { get; set; }*/
        public string? DesignerName { get; set; }

        public string? Descritpion { get; set; }

        public string? ConfiguratorLink { get; set; }
    }
}
