using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using QuickyFUR.Core.Contracts;
using QuickyFUR.Core.Messages;
using QuickyFUR.Core.Models;
using QuickyFUR.Core.Services;
using QuickyFUR.Infrastructure.Data.Models;
using QuickyFUR.Infrastructure.Data.Models.Identity;
using QuickyFUR.Infrastructure.Data.Repositories;
using System;
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
        public void AddProductAsyncThrowsWhenEmptyProduct()
        {
            CreateProductViewModel product = null;

            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.CatchAsync<ArgumentException>(async () => await service.AddProductAsync(product, "8ba62815-82b3-4bd4-8592-efbf58a349c9"), ErrorMessages.modelIsEmpty);
        }
        [Test]
        public void AddProductAsyncAddsProduct()
        {
            CreateProductViewModel product = new CreateProductViewModel()
            {
                Name = "Product",
                Category = "Tables",
                ImageLink = "link",
                Descritpion = "Description",
                ConfiguratorLink = "Link"

            };

            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.DoesNotThrowAsync(async () => await service.AddProductAsync(product, "8ba62815-82b3-4bd4-8592-efbf58a349c9"));
        }

        [Test]
        public void DeleteProductAsyncThrowsWhenEmptyProduct()
        {
            var porductId = 2;

            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.CatchAsync<InvalidOperationException>(async () => await service.DeleteProductAsync(porductId), ErrorMessages.modelNotFound);
        }

        [Test]
        public void DeleteProductAsyncSetDeletedTrueProduct()
        {
            var porductId = 1;

            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.DoesNotThrowAsync(async () => await service.DeleteProductAsync(porductId));
        }

        [Test]
        public void EditDesignerProfileThrowsWhenModelIsNull()
        {
            EditDesignerProfileViewModel model = null;

            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.ThrowsAsync<ArgumentException>(async () => await service.EditDesignerProfileAsync(model, "8ba62815-82b3-4bd4-8592-efbf58a349c9"), ErrorMessages.modelNotFound);
        }

        [Test]
        public void EditDesignerProfileThrowsWhenUserIdIsNull()
        {
            EditDesignerProfileViewModel model = new EditDesignerProfileViewModel()
            {
                Country = "Country",
                Age = 30,
                Autobiography = "Autobiography"
            };

            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.ThrowsAsync<ArgumentException>(async () => await service.EditDesignerProfileAsync(model, null), ErrorMessages.emptyParameter);
        }

        [Test]
        public void EditDesignerProfileThrowsWhenDesignerNotFound()
        {
            EditDesignerProfileViewModel model = new EditDesignerProfileViewModel()
            {
                Country = "Country",
                Age = 30,
                Autobiography = "Autobiography"
            };

            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.EditDesignerProfileAsync(model, "9ba62815-82b3-4bd4-8592-efbf58a349c9"), ErrorMessages.modelNotFound);
        }

        [Test]
        public void EditDesignerProfileEditsDesignerProfile()
        {
            EditDesignerProfileViewModel model = new EditDesignerProfileViewModel()
            {
                Country = "Country",
                Age = 30,
                Autobiography = "Autobiography"
            };

            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.DoesNotThrowAsync(async () => await service.EditDesignerProfileAsync(model, "8ba62815-82b3-4bd4-8592-efbf58a349c9"));
        }

        [Test]
        public void EditProductAsyncThrowWhenModelIsNull()
        {
            EditProductViewModel model = null;

            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.ThrowsAsync<ArgumentException>(async () => await service.EditProductAsync(model, 1), ErrorMessages.modelIsEmpty);
        }

        [Test]
        public void EditProductAsyncThrowWhenProductIdIsNull()
        {
            EditProductViewModel model = new EditProductViewModel()
            {
                Name = "Product",
                Category = "Tables",
                Descritpion = "Description",
                ImageLink = "Image link",
                ConfiguratorLink = "Configuration link"
            };

            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.EditProductAsync(model, 2), ErrorMessages.modelNotFound);
        }

        [Test]
        public void EditProductShouldEditProduct()
        {
            EditProductViewModel model = new EditProductViewModel()
            {
                Name = "Product",
                Category = "Tables",
                Descritpion = "Description",
                ImageLink = "Image link",
                ConfiguratorLink = "Configuration link"
            };

            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.DoesNotThrowAsync(async () => await service.EditProductAsync(model, 1));
        }

        [Test]
        public void GetDesignerAsyncThrowWhenDesignerNotFound()
        {
            string userId = null;

            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.GetDesignerAsync(userId), ErrorMessages.modelNotFound);
        }

        [Test]
        public void GetDesignerAsyncReturnsDesigner()
        {
            string userId = "8ba62815-82b3-4bd4-8592-efbf58a349c9";

            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.DoesNotThrowAsync(async () => await service.GetDesignerAsync(userId));
        }

        [Test]
        public void GetOrdersAsyncReturns()
        {
            string userId = "8ba62815-82b3-4bd4-8592-efbf58a349c9";

            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.DoesNotThrowAsync(async () => await service.GetOrdersAsync(userId));
        }

        [Test]
        public void GetProductForDeleteAsyncThrowsWhenProductNotFound()
        {
            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.GetProductForDeleteAsync(2));
        }

        [Test]
        public void GetProductForDeleteAsyncGetsProduct()
        {
            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.DoesNotThrowAsync(async () => await service.GetProductForDeleteAsync(1));
        }

        [Test]
        public void GetProductForEditAsyncThrowsWhenProductNotFound()
        {
            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.GetProductForEditAsync(2));
        }

        [Test]
        public void GetProductForEditAsyncGetsProduct()
        {
            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.DoesNotThrowAsync(async () => await service.GetProductForEditAsync(1));
        }

        [Test]
        public void MyProductsReturns()
        {
            string userId = "8ba62815-82b3-4bd4-8592-efbf58a349c9";

            var service = _serviceProvider.GetService<IDesignerService>();

            Assert.DoesNotThrow( () => service.MyProducts(userId));
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
                Id = "8ba62815-82b3-4bd4-8592-efbf58a349c9",
                FirstName = "Ivan",
                LastName = "Ivanov"
            };
            var designer = new Designer()
            {
                Id = "3b37d723-c04d-4022-a33d-5c999dc0c3f5",
                ApplicationUser = applicationUser,
                Country = "Bulgaria",
                Age = 30,
                Autobiography = "Autobiography"
            };

            var tableCategory = new Category()
            {
                Id = 1,
                Name = "Tables"
            };

            var chairCategory = new Category()
            {
                Id = 2,
                Name = "Chairs"
            };

            var product = new Product()
            {
                Id = 1,
                Name = "Product",
                CategoryId = 1,
                DesignerId = "3b37d723-c04d-4022-a33d-5c999dc0c3f5",
                Descritpion = "Description",
                ImageLink = "Image link",
                ConfiguratorLink = "Configuration link"
            };

            await repo.AddAsync(applicationUser);
            await repo.AddAsync(designer);
            await repo.AddAsync(tableCategory);
            await repo.AddAsync(chairCategory);
            await repo.AddAsync(product);
            await repo.SaveChangesAsync();
        }
    }
}