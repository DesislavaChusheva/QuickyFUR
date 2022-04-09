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
            var products = _repo.All<ConfiguratedProduct>()
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
            decimal totalPrice = products.Sum(p => p.Price);
            return new CartViewModel()
            {
                Products = products.ToList(),
                TotalPrice = totalPrice
            };
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

        public async Task<bool> OrderProduct(string productJSON, int productId, string userId)
        {
            var productForConfiguration = _repo.All<Product>()
                                               .Where(p => p.Id == productId)
                                               .First();

            var configuratedProductAddedInformation = JsonConvert.DeserializeObject<ImportConfiguratedProductParametersViewModel>(productJSON);

            if (configuratedProductAddedInformation == null)
            {
                return false;
            }

            bool priceParsed = Decimal.TryParse(configuratedProductAddedInformation.Price, out decimal price);
            if (!priceParsed)
            {
                return false;
            }


            var configuratedProduct = new ConfiguratedProduct()
            {
                Name = productForConfiguration.Name,
                CategoryId = productForConfiguration.CategoryId,
                ImageLink = productForConfiguration.ImageLink,
                DesignerId = productForConfiguration.DesignerId,
                Descritpion = productForConfiguration.Descritpion,
                Dimensions = configuratedProductAddedInformation.Dimensions,
                Additions = configuratedProductAddedInformation.Additions,
                Materials = configuratedProductAddedInformation.Materials,
                Price = price
            };


            Customer customer = _repo.All<Customer>()
                                     .Where(c => c.ApplicationUser.Id == userId)
                                     .First();
            string? customerCartId = customer.CartId;

            if (customerCartId == null)
            {
                Cart cart = new Cart();

                cart = new Cart()
                {
                    Customer = customer
                };

                customerCartId = cart.Id;

                await _repo.AddAsync(cart);
            }

            configuratedProduct.CartId = customerCartId;

            await _repo.AddAsync(configuratedProduct);

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
