using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using QuickyFUR.Core.Contracts;
using QuickyFUR.Core.Services;
using QuickyFUR.Infrastructure.Data.Models;
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
            /*var designer = new Designer()
            {
                 
            };*/
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}