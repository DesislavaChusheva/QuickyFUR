using QuickyFUR.Core.Contracts;
using QuickyFUR.Core.Models;
using QuickyFUR.Infrastructure.Data.Models;
using QuickyFUR.Infrastructure.Data.Repositories;
using Newtonsoft.Json;
using QuickyFUR.Core.Messages;

namespace QuickyFUR.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IApplicationDbRepository _repo;


        public CustomerService(IApplicationDbRepository repo)
        {
            _repo = repo;
        }

        /*        public async Task<decimal> GetCartTotalPriceAsync(string cartId)
                {
                    Cart cart = _repo.All<Cart>()
                                .Where(c => c.Id == cartId)
                                .First();

                    if (cart == null)
                    {
                        throw new InvalidOperationException(ErrorMessages.modelNotFound);
                    }

                    decimal totalPrice = cart.Products.Sum(c => c.Price);

                    return totalPrice;
                }*/

        public async Task<CartViewModel> GetCartAsync(string userId)
        {
            string cartId = _repo.All<Customer>()
                                .Where(c => c.ApplicationUser.Id == userId)
                                .Select(c => c.CartId == null ? null : c.CartId)
                                .First();

            if (cartId == null)
            {
                return null;
            }

            var cart = _repo.All<Cart>()
                            .Where(c => c.Customer.ApplicationUser.Id == userId)
                            .First();

            var count = _repo.All<ConfiguratedProduct>()
                             .Where(p => p.Cart.Customer.ApplicationUser.Id == userId && p.Sold == false && p.Removed == false)
                             .ToList()
                             .Count();
            
            if (count == 0)
            {
                return null;
            }

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
            var product = _repo.All<Product>()
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

            if (product == null)
            {
                throw new InvalidOperationException(ErrorMessages.modelNotFound);
            }

            return product;
        }

        public async Task<bool> OrderProductAsync(string productJSON, int productId, string userId)
        {
            var productForConfiguration = _repo.All<Product>()
                                               .Where(p => p.Id == productId && p.Deleted == false)
                                               .First();

            if (productForConfiguration == null)
            {
                throw new InvalidOperationException(ErrorMessages.modelNotFound);
            }

            var configuratedProductAddedInformation = JsonConvert.DeserializeObject<ImportConfiguratedProductParametersViewModel>(productJSON);

            if (configuratedProductAddedInformation == null)
            {
                throw new ArgumentException(ErrorMessages.emptyParameter);
            }

            bool priceParsed = Decimal.TryParse(configuratedProductAddedInformation.Price, out decimal price);
            if (!priceParsed)
            {
                throw new ArgumentException(ErrorMessages.emptyParameter);
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

            if (productForConfiguration == null)
            {
                throw new InvalidOperationException(ErrorMessages.modelNotFound);
            }

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
            Cart cartC = _repo.All<Cart>()
                             .Where(c => c.Id == customerCartId)
                             .First();
            cartC.Products.Add(configuratedProduct);

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

            if (productsInCart.ToList().Count == 0)
            {
                throw new InvalidOperationException(ErrorMessages.modelNotFound);
            }

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

            if (designerId == null)
            {
                throw new InvalidOperationException(ErrorMessages.modelNotFound);
            }

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
            if (designerId == null)
            {
                throw new ArgumentException(ErrorMessages.emptyParameter);
            }
            
            var products = _repo.All<Product>()
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

            if (products.ToList().Count == 0)
            {
                throw new InvalidOperationException(ErrorMessages.modelNotFound);
            }

            return products;
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

            if (product == null)
            {
                throw new InvalidOperationException(ErrorMessages.modelNotFound);
            }

            return product;
        }

        public async Task<bool> RemoveProductAsync(int productId)
        {
            var product = _repo.All<ConfiguratedProduct>()
              .Where(p => p.Id == productId)
              .First();

            if (product == null)
            {
                throw new InvalidOperationException(ErrorMessages.modelNotFound);
            }

            product.Removed = true;
            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<EditCustomerProfileViewModel> GetCustomerAsync(string userId)
        {
            var customer = _repo.All<Customer>()
                                .Where(c => c.ApplicationUser.Id == userId)
                                .Select(c => new EditCustomerProfileViewModel()
                                {
                                    Address = c.Address
                                })
                                .First();

            if (customer == null)
            {
                throw new InvalidOperationException(ErrorMessages.modelNotFound);
            }

            return customer;
        }

        public async Task<bool> EditCustomerProfile(EditCustomerProfileViewModel model, string userId)
        {
            if (model == null)
            {
                throw new ArgumentException(ErrorMessages.modelIsEmpty);
            }

            var customer = _repo.All<Customer>()
                                .Where(c => c.ApplicationUser.Id == userId)
                                .First();

            if (customer == null)
            {
                throw new InvalidOperationException(ErrorMessages.modelNotFound);
            }

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
