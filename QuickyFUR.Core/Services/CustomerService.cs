using QuickyFUR.Core.Contracts;
using QuickyFUR.Core.Models;
using QuickyFUR.Infrastructure.Data.Models;
using QuickyFUR.Infrastructure.Data.Repositories;
using Newtonsoft.Json;


namespace QuickyFUR.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IApplicationDbRepository _repo;


        public CustomerService(IApplicationDbRepository repo)
        {
            _repo = repo;
        }

        public async Task<decimal> GetCartTotalPrice(string cartId)
        {
            Cart cart = _repo.All<Cart>()
                        .Where(c => c.Id == cartId)
                        .First();

            decimal totalPrice = cart.Products.Sum(c => c.Price);

            return totalPrice;
        }

        public IEnumerable<ProductsInCartViewModel> GetCart(string cartId)
        {
            return _repo.All<ConfiguratedProduct>()
                        .Where(p => p.CartId == cartId)
                        .Select(p => new ProductsInCartViewModel()
                        {
                            Name = p.Name,
                            Category = p.Category.Name,
                            ImageLink = p.ImageLink,
                            DesignerName = p.Designer.ApplicationUser.FullName,
                            Descritpion = p.Descritpion,
                            Dimensions = p.Dimensions,
                            Materials = p.Materials,
                            Price = p.Price
                        });
        }

        public async Task<OrderProductViewModel> GetProductForOrder(int productId)
        {
            return _repo.All<Product>()
                        .Where(p => p.Id == productId)
                        .Select(p => new OrderProductViewModel()
                        {
                            Name = p.Name,
                            Category = p.Category.Name,
                            ImageLink = p.ImageLink,
                            DesignerName = p.Designer.ApplicationUser.FullName,
                            Descritpion = p.Descritpion,
                            ConfiguratorLink = p.ConfiguratorLink
                        })
                        .First();
        }

        public async Task<bool> OrderProduct(string productJSON, string userId)
        {
            var configuratedProduct = JsonConvert.DeserializeObject<ConfiguratedProduct>(productJSON);
            if (configuratedProduct == null)
            {
                return false;
            }

            Customer customer = _repo.All<Customer>()
                                     .Where(c => c.ApplicationUser.Id == userId)
                                     .First();
            Cart? customerCart = customer.Cart;

            if (customerCart == null)
            {
                customerCart = new Cart()
                {
                    Customer = customer
                };

                await _repo.AddAsync(customerCart);
            }

            await _repo.AddAsync(configuratedProduct);

            customerCart.Products.Add(configuratedProduct);
            configuratedProduct.Cart = customerCart;

            await _repo.SaveChangesAsync();

            return true;
        }

        public IEnumerable<AllProductsByCategoryViewModel> ProductsByCategory(int categoryId)
        {
            return _repo.All<Product>()
                        .Where(p => p.CategoryId == categoryId)
                        .Select(p => new AllProductsByCategoryViewModel()
                        {
                            Name = p.Name,
                            ImageLink = p.ImageLink,
                            DesignerName = p.Designer.ApplicationUser.FullName,
                            Descritpion = p.Descritpion,
                            ConfiguratorLink = p.ConfiguratorLink
                        });
        }

        public async Task<bool> BuyProductsFromCart(string cartId)
        {
            Cart cart = _repo.All<Cart>()
                             .Where(c => c.Id == cartId)
                             .First();

            cart.Products.Clear();
            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<DesignerInfoViewModel> GetDesignerInfoForThisProduct(int productId)
        {
            var designerId = _repo.All<Product>()
                                  .Where(p => p.Id == productId)
                                  .Select(p => p.DesignerId)
                                  .First();

            var designer = _repo.All<Designer>()
                        .Where(d => d.Id == designerId)
                        .Select(d => new DesignerInfoViewModel()
                        {
                            FullName = d.ApplicationUser.FullName,
                            Country = d.Country,
                            Age = d.Age,
                            Autobiography = d.Autobiography,
                            Products = GetProductsForThisDesigner(designerId).ToList()
                        })
                        .ToList()
                        .First();

            return designer;
        }

        public IEnumerable<AllProductsByDesignerViewModel> GetProductsForThisDesigner(string designerId)
        {
            return _repo.All<Product>()
                        .Where(p => p.DesignerId == designerId)
                        .Select(p => new AllProductsByDesignerViewModel()
                        {
                            Name = p.Name,
                            Category = p.Category.Name,
                            ImageLink = p.ImageLink,
                            Descritpion = p.Descritpion,
                            ConfiguratorLink = p.ConfiguratorLink
                        });
        }
    }
}
