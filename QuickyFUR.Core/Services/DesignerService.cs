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

            Category? category = _repo.All<Category>().FirstOrDefault(c => c.Name == model.Category);
            if (category == null)
            {
                throw new ArgumentException(ErrorMessages.categoryErrorMessage);
            }
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

            product.Deleted = true;
            await _repo.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditDesignerProfile(EditDesignerProfileViewModel model, string userId)
        {
            var designer = _repo.All<Designer>()
                                .Where(d => d.ApplicationUser.Id == userId)
                                .First();

            designer.Country = model.Country;
            designer.Age = model.Age;
            designer.Autobiography = model.Autobiography;

            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditProductAsync(EditProductViewModel model, int productId)
        {
            var product = _repo.All<Product>()
                                      .Where(p => p.Id == productId)
                                      .First();

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
            return _repo.All<Designer>()
                        .Where(d => d.ApplicationUser.Id == userId)
                        .Select(d => new EditDesignerProfileViewModel()
                        {
                            Country = d.Country,
                            Age = d.Age,
                            Autobiography = d.Autobiography
                        })
                        .First();
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
                                   ImageLink= p.ImageLink,
                                   Descritpion= p.Descritpion,
                                   ConfiguratorLink= p.ConfiguratorLink
                               })
                               .First();
            return product;
        }

        public IEnumerable<AllProductsViewModel> MyProducts(string userId)
        {
            return _repo.All<Product>()
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
        }
    }
}
