using QuickyFUR.Core.Contracts;
using QuickyFUR.Core.Models;
using QuickyFUR.Infrastructure.Data.Models;
using QuickyFUR.Infrastructure.Data.Repositories;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using QuickyFUR.Infrastructure.Data.Models.Identity;


namespace QuickyFUR.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IApplicationDbRepository _repo;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;


        public CustomerService(IApplicationDbRepository repo,
                               SignInManager<ApplicationUser> signInManager,   
                               UserManager<ApplicationUser> userManager,
                               IUserStore<ApplicationUser> userStore)
        {
            _repo = repo;
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
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
            var configuratedProduct = JsonConvert.DeserializeObject<IList<ConfiguratedProduct>>(productJSON);
            if (configuratedProduct == null)
            {
                return false;
            }
            await _repo.AddAsync(configuratedProduct);
            await _repo.SaveChangesAsync();

/*            if (await _userStore.FindByIdAsync(userId))
            {

            }*/
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
    }
}
