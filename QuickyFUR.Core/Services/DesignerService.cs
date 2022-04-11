using QuickyFUR.Core.Contracts;
using QuickyFUR.Core.Messages;
using QuickyFUR.Core.Models;
using QuickyFUR.Infrastructure.Data;
using QuickyFUR.Infrastructure.Data.Models;
using QuickyFUR.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickyFUR.Core.Services
{
    public class DesignerService : IDesignerService
    {
        private readonly IApplicationDbRepository _repo;


        public DesignerService(IApplicationDbRepository repo)
        {
            _repo = repo;
        }
        public async Task<bool> AddProductAsync(CreateProductViewModel model, string userId)
        {

            if (model == null)
            {
                throw new ArgumentException(ErrorMessages.modelIsEmpty);
            }

            Category? category = _repo.All<Category>().FirstOrDefault(c => c.Name == model.Category);

            Designer designer = _repo.All<Designer>()
                                     .Where(d => d.ApplicationUser.Id == userId)
                                     .First();

            Product product = new Product()
            {
                Name = model.Name,
                Category = category,
                Designer = designer,
                ImageLink = model.ImageLink,
                Descritpion = model.Descritpion,
                ConfiguratorLink = model.ConfiguratorLink
            };

            await _repo.AddAsync(product);
            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = _repo.All<Product>()
                          .Where(p => p.Id == productId)
                          .First();

            if (product == null)
            {
                throw new InvalidOperationException(ErrorMessages.modelNotFound);
            }

            product.Deleted = true;
            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditDesignerProfile(EditDesignerProfileViewModel model, string userId)
        {
            if (model == null)
            {
                throw new ArgumentException(ErrorMessages.modelIsEmpty);
            }
            if (userId == null)
            {
                throw new ArgumentException(ErrorMessages.emptyParameter);
            }
            var designer = _repo.All<Designer>()
                                .Where(d => d.ApplicationUser.Id == userId)
                                .First();

            if (designer == null)
            {
                throw new InvalidOperationException(ErrorMessages.modelNotFound);
            }

            designer.Country = model.Country;
            designer.Age = model.Age;
            designer.Autobiography = model.Autobiography;

            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditProductAsync(EditProductViewModel model, int productId)
        {
            if (model == null)
            {
                throw new ArgumentException(ErrorMessages.modelIsEmpty);
            }

            var product = _repo.All<Product>()
                                      .Where(p => p.Id == productId)
                                      .First();

            if (product == null)
            {
                throw new InvalidOperationException(ErrorMessages.modelNotFound);
            }

            product.Name = model.Name;
            product.CategoryId = _repo.All<Category>().Where(c => c.Name == model.Category).Select(c => c.Id).First();
            product.ImageLink = model.ImageLink;
            product.Descritpion = model?.Descritpion;
            product.ConfiguratorLink = model?.ConfiguratorLink;

            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<EditDesignerProfileViewModel> GetDesignerAsync(string userId)
        {
            var designer = _repo.All<Designer>()
                                .Where(d => d.ApplicationUser.Id == userId)
                                .Select(d => new EditDesignerProfileViewModel()
                                {
                                    Country = d.Country,
                                    Age = d.Age,
                                    Autobiography = d.Autobiography
                                })
                                .First();

            if (designer == null)
            {
                throw new InvalidOperationException(ErrorMessages.modelNotFound);
            }

            return designer;
        }

        public async Task<List<OrderPageViewModel>> GetOrders(string userId)
        {
            var allOrders = _repo.All<ConfiguratedProduct>()
                                 .Where(p => p.Designer.ApplicationUser.Id == userId && p.Sold == true)
                                 .ToList();

            var cartsIds = allOrders.Select(o => o.CartId)
                                        .ToList()
                                        .Distinct();

            var customersIds = _repo.All<Customer>()
                                    .Where(c => cartsIds.Contains(c.CartId))
                                    .Select(c => c.Id)
                                    .ToList();


            List<OrderPageViewModel> finalOrders = new List<OrderPageViewModel>();

            foreach (var customerId in customersIds)
            {
                var customer = _repo.All<Customer>()
                                    .Where(c => c.Id == customerId)
                                    .Select(d => d.ApplicationUser.FullName)
                                    .First()
                                    .ToString();

                var products = _repo.All<ConfiguratedProduct>()
                                    .Where(p => p.Designer.ApplicationUser.Id == userId && p.Cart.Customer.Id == customerId && p.Sold == true)
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
                    DorCSide = customer,
                    Products = products
                };

                finalOrders.Add(orders);
            }

            return finalOrders.ToList();
        }

        public async Task<DeleteProductViewModel> GetProductForDeleteAsync(int productId)
        {
            var product = _repo.All<Product>()
                   .Where(p => p.Id == productId)
                   .Select(p => new DeleteProductViewModel()
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

        public async Task<EditProductViewModel> GetProductForEditAsync(int productId)
        {
            var product = _repo.All<Product>()
                               .Where(p => p.Id == productId)
                               .Select(p => new EditProductViewModel()
                               {
                                   ProductId = productId,
                                   Name = p.Name,
                                   Category = p.Category.Name,
                                   ImageLink = p.ImageLink,
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

        public IEnumerable<AllProductsViewModel> MyProducts(string userId)
        {
            var products = _repo.All<Product>()
                                .Where(p => p.Designer.ApplicationUser.Id == userId && p.Deleted == false)
                                .Select(p => new AllProductsViewModel()
                                {
                                    ProdcuctId = p.Id,
                                    Name = p.Name,
                                    Category = p.Category.Name,
                                    ImageLink = p.ImageLink,
                                    DesignerName = _repo.All<Designer>()
                                                .First(d => d.ApplicationUser.Id == userId)
                                                .ApplicationUser
                                                .FullName,
                                    Descritpion = p.Descritpion,
                                    ConfiguratorLink = p.ConfiguratorLink
                                });

            return products;
        }
    }
}
