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

        public async Task<decimal> GetCartTotalPriceAsync(string cartId)
        {
            Cart cart = _repo.All<Cart>()
                        .Where(c => c.Id == cartId)
                        .First();

            decimal totalPrice = cart.Products.Sum(c => c.Price);

            return totalPrice;
        }

        public async Task<CartViewModel> GetCartAsync(string userId)
        {
            var cart = _repo.All<Cart>()
                            .Where(c => c.Customer.ApplicationUser.Id == userId)
                            .First();
            var products = _repo.All<ConfiguratedProduct>()
                                .Where(p => p.Cart.Customer.ApplicationUser.Id == userId && p.Sold == false && p.Removed == false)
                                .Select(p => new ProductsInCartViewModel()
                                {
                                    ProductId = p.Id,
                                    Name = p.Name,
                                    Category = p.Category.Name,
                                    ImageLink = p.ImageLink,
                                    DesignerName = p.Designer.ApplicationUser.FullName,
                                    Descritpion = p.Descritpion,
                                    Dimensions = p.Dimensions,
                                    Materials = p.Materials,
                                    Price = p.Price,
                                    Sold = p.Sold
                                });
            decimal totalPrice = products.Sum(p => p.Price);
            return new CartViewModel()
            {
                CartId = cart.Id,
                Products = products.ToList(),
                TotalPrice = totalPrice
            };
        }

        public async Task<OrderProductViewModel> GetProductForOrderAsync(int productId)
        {
            return _repo.All<Product>()
                        .Where(p => p.Id == productId && p.Deleted == false)
                        .Select(p => new OrderProductViewModel()
                        {
                            ProductId = productId,
                            Name = p.Name,
                            Category = p.Category.Name,
                            ImageLink = p.ImageLink,
                            DesignerName = p.Designer.ApplicationUser.FullName,
                            Descritpion = p.Descritpion,
                            ConfiguratorLink = p.ConfiguratorLink
                        })
                        .First();
        }

        public async Task<bool> OrderProductAsync(string productJSON, int productId, string userId)
        {
            var productForConfiguration = _repo.All<Product>()
                                               .Where(p => p.Id == productId && p.Deleted == false)
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

        public IEnumerable<AllProductsViewModel> ProductsByCategoryAsync(string category)
        {
            var products = _repo.All<Product>()
                        .Where(p => p.Category.Name == category && p.Deleted == false)
                        .Select(p => new AllProductsViewModel()
                        {
                            ProdcuctId = p.Id,
                            Name = p.Name,
                            ImageLink = p.ImageLink,
                            Category = p.Category.Name,
                            DesignerName = p.Designer.ApplicationUser.FullName,
                            Descritpion = p.Descritpion,
                            ConfiguratorLink = p.ConfiguratorLink
                        });
            return products;
        }

        public async Task<bool> BuyProductsFromCartAsync(string cartId)
        {
            var productsInCart = _repo.All<ConfiguratedProduct>()
                                      .Where(p => p.CartId == cartId && p.Removed == false);

            foreach (var p in productsInCart)
            {
                p.Sold = true;
            }

            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<DesignerInfoViewModel> GetDesignerInfoForThisProductAsync(int productId)
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
                            Products = GetProductsForThisDesignerAsync(designerId).ToList()
                        })
                        .ToList()
                        .First();

            return designer;
        }

        public IEnumerable<AllProductsByDesignerViewModel> GetProductsForThisDesignerAsync(string designerId)
        {
            return _repo.All<Product>()
                        .Where(p => p.DesignerId == designerId && p.Deleted == false)
                        .Select(p => new AllProductsByDesignerViewModel()
                        {
                            ProductId = p.Id,
                            Name = p.Name,
                            Category = p.Category.Name,
                            ImageLink = p.ImageLink,
                            Descritpion = p.Descritpion,
                            ConfiguratorLink = p.ConfiguratorLink
                        });
        }

        public IEnumerable<AllProductsViewModel> GetAllProductsAsync()
        {
            return _repo.All<Product>()
                        .Where(p => p.Deleted == false)
                        .Select(p => new AllProductsViewModel()
                        {
                            Name = p.Name,
                            Category = p.Category.Name,
                            ImageLink = p.ImageLink,
                            DesignerName = p.Designer.ApplicationUser.FullName,
                            Descritpion = p.Descritpion,
                            ConfiguratorLink = p.ConfiguratorLink
                        });
        }

        public async Task<RemoveProductFromCartViewModel> GetProductForRemoveAsync(int productId)
        {
            var product = _repo.All<ConfiguratedProduct>()
                   .Where(p => p.Id == productId && p.Removed == false)
                   .Select(p => new RemoveProductFromCartViewModel()
                   {
                       ProductId = productId,
                       Name = p.Name
                   })
                   .First();
            return product;
        }

        public async Task<bool> RemoveProductAsync(int productId)
        {
            var product = _repo.All<ConfiguratedProduct>()
              .Where(p => p.Id == productId)
              .First();

            product.Removed = true;
            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<EditCustomerProfileViewModel> GetCustomerAsync(string userId)
        {
            return _repo.All<Customer>()
                        .Where(c => c.ApplicationUser.Id == userId)
                        .Select(c => new EditCustomerProfileViewModel()
                        {
                            Address = c.Address
                        })
                        .First();
        }

        public async Task<bool> EditCustomerProfile(EditCustomerProfileViewModel model, string userId)
        {
            var customer = _repo.All<Customer>()
                                .Where(c => c.ApplicationUser.Id == userId)
                                .First();

            customer.Address = model.Address;
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<List<OrderPageViewModel>> GetOrders(string userId)
        {
            var allOrders = _repo.All<ConfiguratedProduct>()
                                 .Where(p => p.Cart.Customer.ApplicationUser.Id == userId && p.Sold == true)
                                 .ToList();

            var designersIds = allOrders.Select(o => o.DesignerId)
                                        .ToList()
                                        .Distinct();

            List<OrderPageViewModel> finalOrders = new List<OrderPageViewModel>();

            foreach (var designerId in designersIds)
            {
                var designer = _repo.All<Designer>()
                                    .Where(d => d.Id == designerId)
                                    .Select(d => d.ApplicationUser.FullName)
                                    .First()
                                    .ToString();

                var products = _repo.All<ConfiguratedProduct>()
                                    .Where(p => p.Cart.Customer.ApplicationUser.Id == userId && p.DesignerId == designerId && p.Sold == true)
                                    .Select(p => new OrderedProductsViewModel()
                                    {
                                        Name = p.Name,
                                        Dimensions = p.Dimensions,
                                        Additions = p.Additions,
                                        Materials = p.Materials,
                                        Price = p.Price
                                    }).ToList();

                var orders = new OrderPageViewModel()
                                 {
                                     DorCSide = designer,
                                     Products = products
                                 };

                finalOrders.Add(orders);
            }

            return finalOrders.ToList();
        }
    }
}
