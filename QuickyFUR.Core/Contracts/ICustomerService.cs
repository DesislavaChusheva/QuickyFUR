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
        IEnumerable<AllProductsByCategoryViewModel> ProductsByCategory(int categoryId);
        Task<OrderProductViewModel> GetProductForOrder(int productId);
        Task<bool> OrderProduct(string productJSON, string userId);
        IEnumerable<ProductsInCartViewModel> GetCart(string cartId);
        Task<bool> BuyProductsFromCart(string cartId);
        Task<decimal> GetCartTotalPrice(string cartId);
        Task<DesignerInfoViewModel> GetDesignerInfoForThisProduct(int productId);
        IEnumerable<AllProductsByDesignerViewModel> GetProductsForThisDesigner(string designerId);
    }
}
