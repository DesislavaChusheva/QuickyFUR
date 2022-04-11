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
        IEnumerable<AllProductsViewModel> MyProducts(string userId);
        Task<bool> AddProductAsync(CreateProductViewModel model, string userId);
        Task<bool> EditProductAsync(EditProductViewModel model, int productId);
        Task<bool> DeleteProductAsync(int productId);
        Task<EditProductViewModel> GetProductForEditAsync(int productId);
        Task<DeleteProductViewModel> GetProductForDeleteAsync(int productId);
        Task<EditDesignerProfileViewModel> GetDesignerAsync(string userId);
        Task<bool> EditDesignerProfile(EditDesignerProfileViewModel model, string userId);
        Task<List<OrderPageViewModel>> GetOrders(string userId);
    }
}
