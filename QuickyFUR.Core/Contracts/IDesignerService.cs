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
        IEnumerable<AllProductsViewModel> MyProducts(string designerId);
        Task<bool> AddProductAsync(CreateProductViewModel model);
        Task<EditProductViewModel> EditProductAsync(string productId);
    }
}
