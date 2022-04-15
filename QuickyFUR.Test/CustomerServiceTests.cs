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
    public class CustomerServiceTests
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
                .AddSingleton<ICustomerService, CustomerService>()
                .BuildServiceProvider();

            var repo = _serviceProvider.GetService<IApplicationDbRepository>();

            await SeedDbAsync(repo);

        }

        [Test]
        public void GetCartAsyncReturns()
        {
            string userId = "e60961e7-6473-4c9b-bacf-00196946c824";

            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.DoesNotThrowAsync(async () => await service.GetCartAsync(userId));
        }

        [Test]
        public void GetProductForOrderAsyncThrowsWnenProductNotFound()
        {
            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.GetProductForOrderAsync(2), ErrorMessages.modelNotFound);
        }

        [Test]
        public void GetProductForOrderAsyncGetsProduct()
        {
            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.DoesNotThrowAsync(async () => await service.GetProductForOrderAsync(1));
        }

        [Test]
        public void OrderProductAsyncThrowsWhenProductNotFound()
        {
            string JSON = @"{
  ""Dimensions"": ""354*959*532"",
  ""Additions"": ""Sharp/Rectangular"",
  ""Materials"": ""Aluminium/Glass"",
  ""Price"": ""348.8525""
}";
            string userId = "e60961e7-6473-4c9b-bacf-00196946c824";

            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.OrderProductAsync(JSON, 4, userId), ErrorMessages.modelNotFound);
        }

        [Test]
        public void OrderProductAsyncThrowsWhenJSONNotCorrect()
        {
            string JSON = @"{""SomethingKey"" : ""SomethingValue""}";

            string userId = "e60961e7-6473-4c9b-bacf-00196946c824";

            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.ThrowsAsync<ArgumentException>(async () => await service.OrderProductAsync(JSON, 1, userId), ErrorMessages.emptyParameter);
        }

        [Test]
        public void OrderProductAsyncThrowsWhenPriceNotParsed()
        {
            string JSON = @"{
  ""Dimensions"": ""354*959*532"",
  ""Additions"": ""Sharp/Rectangular"",
  ""Materials"": ""Aluminium/Glass"",
  ""Price"": ""Price""
}";
            string userId = "e60961e7-6473-4c9b-bacf-00196946c824";

            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.ThrowsAsync<ArgumentException>(async () => await service.OrderProductAsync(JSON, 1, userId), ErrorMessages.emptyParameter);
        }

        [Test]
        public void OrderProductAsyncAddsToCart()
        {
            string JSON = @"{
  ""Dimensions"": ""354*959*532"",
  ""Additions"": ""Sharp/Rectangular"",
  ""Materials"": ""Aluminium/Glass"",
  ""Price"": ""348.8525""
}";

            string userId = "e60961e7-6473-4c9b-bacf-00196946c824";

            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.DoesNotThrowAsync(async () => await service.OrderProductAsync(JSON, 1, userId));
        }

        [Test]
        public void OrderProductAsyncCreatesAndAddsToCart()
        {
            string JSON = @"{
  ""Dimensions"": ""354*959*532"",
  ""Additions"": ""Sharp/Rectangular"",
  ""Materials"": ""Aluminium/Glass"",
  ""Price"": ""348.8525""
}";

            string userId = "486b1d6e-13ad-422e-ad27-68ce92c47b1f";

            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.DoesNotThrowAsync(async () => await service.OrderProductAsync(JSON, 1, userId));
        }


        [Test]
        public void ProductsByCategoryAsyncReturns()
        {
            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.DoesNotThrow(() => service.ProductsByCategoryAsync("Tables"));
        }

        [Test]
        public void ProductsByCategoryAsyncReturnsNull()
        {
            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.DoesNotThrow(() => service.ProductsByCategoryAsync(" "));
        }

        [Test]
        public void BuyProductsFromCartAsyncThrowsWhenNoProducts()
        {
            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.BuyProductsFromCartAsync(null), ErrorMessages.modelNotFound);
        }

        [Test]
        public void BuyProductsAsyncOrdersProducts()
        {
            string cartId = "1b51ae16-2201-45a6-b608-cabfa5a59efb";

            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.DoesNotThrowAsync(async () => await service.BuyProductsFromCartAsync(cartId));
        }

        [Test]
        public void GetDesignerInfoForThisProductAsyncThrowsWhenDesignerNotFound()
        {
            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.GetDesignerInfoForThisProductAsync(4), ErrorMessages.modelNotFound);
        }

        [Test]
        public void GetDesignerInfoForThisProductAsyncReturns()
        {
            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.DoesNotThrowAsync(async () => await service.GetDesignerInfoForThisProductAsync(1));
        }

        [Test]
        public void GetProductsForThisDesignerAsyncThrowsWhenEmptyDesignerId()
        {
            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.Throws<ArgumentException>(() => service.GetProductsForThisDesignerAsync(null), ErrorMessages.emptyParameter);
        }

        [Test]
        public void GetProductsForThisDesignerAsyncThrowsWhenNoProducts()
        {
            string designerId = "4b37d723-c04d-4022-a33d-5c999dc0c3f5";

            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.Throws<InvalidOperationException>(() => service.GetProductsForThisDesignerAsync(designerId), ErrorMessages.modelNotFound);
        }

        [Test]
        public void GetProductsForThisDesignerAsyncReturns()
        {
            string designerId = "3b37d723-c04d-4022-a33d-5c999dc0c3f5";

            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.DoesNotThrow(() => service.GetProductsForThisDesignerAsync(designerId));
        }

        [Test]
        public void GetAllProductsAsyncReturns()
        {
            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.DoesNotThrow(() => service.GetAllProductsAsync());
        }

        [Test]
        public void GetProductForRemoveAsyncThrowsWhenProductNotFound()
        {
            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.GetProductForRemoveAsync(4), ErrorMessages.modelNotFound);
        }

        [Test]
        public void GetProductForeRemoveAsyncRetirns()
        {
            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.DoesNotThrowAsync(async () => await service.GetProductForRemoveAsync(1));
        }

        [Test]
        public void RemoveProductAsyncThrowsWhenProductNotFound()
        {
            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.RemoveProductAsync(4), ErrorMessages.modelNotFound);
        }

        [Test]
        public void RemoveProductAsyncRetirns()
        {
            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.DoesNotThrowAsync(async () => await service.RemoveProductAsync(1));
        }

        [Test]
        public void GetCustomerAsyncThrowsWhenCustomerNotFound()
        {
            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.GetCustomerAsync(null), ErrorMessages.modelNotFound);
        }

        [Test]
        public void GetCustomerReturns()
        {
            string userId = "e60961e7-6473-4c9b-bacf-00196946c824";

            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.DoesNotThrowAsync(async () => await service.GetCustomerAsync(userId));
        }

        [Test]
        public void EditCustomerProfileThrowsWhenModelIsEmpty()
        {
            string userId = "e60961e7-6473-4c9b-bacf-00196946c824";

            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.ThrowsAsync<ArgumentException>(async () => await service.EditCustomerProfile(null, userId), ErrorMessages.modelIsEmpty);
        }

        [Test]
        public void EditCustomerProfileThrowsWhenCustomerNorFound()
        {
            EditCustomerProfileViewModel model = new EditCustomerProfileViewModel()
            {
                Address = "NewAddress"
            };

            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.ThrowsAsync<InvalidOperationException>(async () => await service.EditCustomerProfile(model, null), ErrorMessages.modelNotFound);
        }

        [Test]
        public void EditCustomerProfileReturns()
        {
            string userId = "e60961e7-6473-4c9b-bacf-00196946c824";
            EditCustomerProfileViewModel model = new EditCustomerProfileViewModel()
            {
                Address = "NewAddress"
            };

            var service = _serviceProvider.GetService<ICustomerService>();

            Assert.DoesNotThrowAsync(async () => await service.EditCustomerProfile(model, userId));
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

            var applicationUser2 = new ApplicationUser()
            {
                Id = "9ba62815-82b3-4bd4-8592-efbf58a349c9",
                FirstName = "Ivan",
                LastName = "Ivanov"
            };

            var applicationUserCart = new ApplicationUser()
            {
                Id = "e60961e7-6473-4c9b-bacf-00196946c824",
                FirstName = "Maria",
                LastName = "Ivanova"
            };

            var applicationUserWithoutCart = new ApplicationUser()
            {
                Id = "486b1d6e-13ad-422e-ad27-68ce92c47b1f",
                FirstName = "Andrey",
                LastName = "Andreev"
            };

            var customer = new Customer()
            {
                Id = "3b37d723-c04d-4022-a33d-5c999dc0c3f5",
                ApplicationUser = applicationUserCart,
                Address = "Address"
            };

            var customerWithoutCart = new Customer()
            {
                Id = "fed317a8-c861-4df7-9976-9889a6db85c7",
                ApplicationUser = applicationUserWithoutCart,
                Address = "Address"
            };

            var designer = new Designer()
            {
                Id = "3b37d723-c04d-4022-a33d-5c999dc0c3f5",
                ApplicationUser = applicationUser,
                Country = "Bulgaria",
                Age = 30,
                Autobiography = "Autobiography"
            };

            var designer2 = new Designer()
            {
                Id = "4b37d723-c04d-4022-a33d-5c999dc0c3f5",
                ApplicationUser = applicationUser2,
                Country = "Bulgaria",
                Age = 30,
                Autobiography = "Autobiography"
            };

            var cart = new Cart()
            {
                Id = "1b51ae16-2201-45a6-b608-cabfa5a59efb",
                Customer = customer
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

            var configuratedProduct = new ConfiguratedProduct()
            {
                Id = 1,
                Name = "Product",
                CategoryId = 1,
                DesignerId = "3b37d723-c04d-4022-a33d-5c999dc0c3f5",
                Descritpion = "Description",
                ImageLink = "Image link",
                Dimensions = "Dimensions",
                Materials = "Materials",
                Additions = "Additions",
                Price = 50.00M,
                CartId = "1b51ae16-2201-45a6-b608-cabfa5a59efb"
            };


            await repo.AddAsync(applicationUser);
            await repo.AddAsync(applicationUser2);
            await repo.AddAsync(applicationUserCart);
            await repo.AddAsync(applicationUserWithoutCart);
            await repo.AddAsync(customer);
            await repo.AddAsync(customerWithoutCart);
            await repo.AddAsync(designer);
            await repo.AddAsync(designer2);
            await repo.AddAsync(cart);
            await repo.AddAsync(tableCategory);
            await repo.AddAsync(chairCategory);
            await repo.AddAsync(product);
            await repo.AddAsync(configuratedProduct);
            await repo.SaveChangesAsync();
        }
    }
}