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
        public async Task<bool> AddProductAsync(CreateProductViewModel model)
        {

            Category? category = _repo.All<Category>().FirstOrDefault(c => c.Name == model.Category);
            if (category == null)
            {
                throw new ArgumentException(ErrorMessages.categoryErrorMessage);
            }

            Product product = new Product()
            {
                Name = model.Name,
                Category = category,
                Image = model.Image,
                Descritpion = model.Descritpion,
                ConfiguratorLink = model.ConfiguratorLink
            };

            await _repo.AddAsync(product);
            await _repo.SaveChangesAsync();

            return true;
        }

        public Task<EditProductViewModel> EditProductAsync(string productId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AllProductsViewModel> MyProducts(string designerId)
        {
            return _repo.All<Product>()
                        .Where(p => p.DesignerId == designerId)
                        .Select(p => new AllProductsViewModel()
                        {
                            Name = p.Name,
                            Category = p.Category.Name,
                            Image = p.Image,
                            DesignerName = _repo.All<Designer>()
                                                .First(d => d.Id == designerId)
                                                .ApplicationUser
                                                .FullName,
                            Descritpion = p.Descritpion,
                            ConfiguratorLink = p.ConfiguratorLink
                        });
        }
    }
}
