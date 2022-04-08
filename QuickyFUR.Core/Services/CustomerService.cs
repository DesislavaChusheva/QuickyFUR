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

        public async Task<CartViewModel> GetCart(string cartId)
        {
            return _repo.All<Cart>()
                        .Where(c => c.Id == cartId)
                        .Select(c => new CartViewModel()
                        {
                            Products = c.Products,
                        })
                        .First();
        }

        public async Task<OrderProductViewModel> GetProductForOrder(int productId)
        {
            return _repo.All<Product>()
                        .Where(p => p.Id == productId)
                        .Select(p => new OrderProductViewModel()
                        {
                            Name = p.Name,
                            Category = p.Category.Name,
                            Image = p.Image,
                            Designer = p.Designer.ApplicationUser.FullName,
                            Descritpion = p.Descritpion
                        })
                        .First();
        }

        public async Task<bool> OrderProductAsync(string productJSON, string userId)
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

        public IEnumerable<AllProductsViewModel> ProductsByCategory(int categoryId)
        {
            return _repo.All<Product>()
                        .Where(p => p.CategoryId == categoryId)
                        .Select(p => new AllProductsViewModel()
                        {
                            Name = p.Name,
                            Category = p.Category.Name,
                            Image = p.Image,
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
            return _repo.All<Designer>()
                        .Where(d => d.Products
                                     .Any(p => p.Id == productId))
                        .Select(d => new DesignerInfoViewModel()
                        {
                            FullName = d.ApplicationUser.FullName,
                            Country = d.Country,
                            Age = d.Age,
                            Autobiography = d.Autobiography,
                            Categories = d.Categories,
                            Products = d.Products
                        })
                        .First();
        }

        public IEnumerable<AllProductsViewModel> GetProductsForThisDesigner(string designerId)
        {
            return _repo.All<Product>()
                        .Where(p => p.DesignerId == designerId)
                        .Select(p => new AllProductsViewModel()
                        {
                            Name = p.Name,
                            Category = p.Category.Name,
                            Image = p.Image,
                            DesignerName = p.Designer.ApplicationUser.FullName,
                            Descritpion = p.Descritpion,
                            ConfiguratorLink = p.ConfiguratorLink
                        });
        }
    }
}
