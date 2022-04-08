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
        IEnumerable<AllProductsViewModel> ProductsByCategory(int categoryId);
        Task<OrderProductViewModel> GetProductForOrder(int productId);
        Task<bool> OrderProductAsync(string productJSON, string userId);
        Task<CartViewModel> GetCart(string cartId);
        Task<bool> BuyProductsFromCart(string cartId);
        Task<decimal> GetCartTotalPrice(string cartId);
        Task<DesignerInfoViewModel> GetDesignerInfoForThisProduct(int productId);
        IEnumerable<AllProductsViewModel> GetProductsForThisDesigner(string designerId);
    }
}
