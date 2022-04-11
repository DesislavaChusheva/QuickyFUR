using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using QuickyFUR.Core.Contracts;
using QuickyFUR.Core.Services;
using QuickyFUR.Infrastructure.Data.Models;
using QuickyFUR.Infrastructure.Data.Models.Identity;
using QuickyFUR.Infrastructure.Data.Repositories;
using System.Threading.Tasks;

namespace QuickyFUR.Test
{
    public class DesignerServiceTests
    {
        private ServiceProvider _serviceProvider;
        private InMemoryDbContext _dbContext;
       
        [SetUp]
        public async Task Setup()
        {
            _dbContext = new InMemoryDbContext();

            var serviceCollection = new ServiceCollection();
            _serviceProvider = serviceCollection
                .AddSingleton(sp => _dbContext.CreateContext())
                .AddSingleton<IApplicationDbRepository, ApplicationDbRepository>()
                .AddSingleton<IDesignerService, DesignerService>()
                .BuildServiceProvider();

            var repo = _serviceProvider.GetService<IApplicationDbRepository>();

            await SeedDbAsync(repo);

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        private async Task SeedDbAsync(IApplicationDbRepository repo)
        {
            var applicationUser = new ApplicationUser()
            {
                FirstName = "Ivan",
                LastName = "Ivanov"
            };
            var designer = new Designer()
            {
                ApplicationUser = applicationUser,
                Country = "Bulgaria",
                Age = 30,
                Autobiography = "Autobiography"
            };

            await repo.AddAsync(applicationUser);
            await repo.AddAsync(designer);
            await repo.SaveChangesAsync();
        }
    }
}