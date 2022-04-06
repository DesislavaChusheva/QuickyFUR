using QuickyFUR.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickyFUR.Core.Contracts
{
    public interface IDesignerService
    {
        IEnumerable<AllProductsViewModel> AllProducts();
        Task<bool> AddProductAsync(CreateProductViewModel model);
    }
}
