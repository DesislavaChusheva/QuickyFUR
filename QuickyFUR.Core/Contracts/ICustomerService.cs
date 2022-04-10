using QuickyFUR.Core.Models;
using QuickyFUR.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickyFUR.Core.Contracts
{
    public interface ICustomerService
    {
        IEnumerable<AllProductsViewModel> ProductsByCategoryAsync(string category);
        Task<OrderProductViewModel> GetProductForOrderAsync(int productId);
        Task<bool> OrderProductAsync(string productJSON, int productId, string userId);
        Task<CartViewModel> GetCartAsync(string userId);
        Task<bool> BuyProductsFromCartAsync(string cartId);
        Task<decimal> GetCartTotalPriceAsync(string cartId);
        Task<DesignerInfoViewModel> GetDesignerInfoForThisProductAsync(int productId);
        IEnumerable<AllProductsByDesignerViewModel> GetProductsForThisDesignerAsync(string designerId);
        IEnumerable<AllProductsViewModel> GetAllProductsAsync();
        Task<RemoveProductFromCartViewModel> GetProductForRemoveAsync(int productId);
        Task<bool> RemoveProductAsync(int productId);
    }
}
