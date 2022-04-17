/*using QuickyFUR.Infrastructure.Data.Models.Identity;
using QuickyFUR.Infrastructure.Data.Repositories;
using QuickyFUR.Areas.Identity.Controllers;
using Microsoft.AspNetCore.Identity;
using Xunit;
//using Moq;
using QuickyFUR.Tests.Mock;

namespace QuickyFUR.Test
{
    public class CustomerControllerTests
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Mock<IApplicationDbRepository> _mockRepo;
        private readonly CustomerController _controller;
        public CustomerControllerTests()
        {
            _mockRepo = new Mock<IApplicationDbRepository>();
            _controller = new CustomerController(_mockRepo.Object);
            _roleManager = new RoleManager<IdentityRole>();
            _userManager = new UserManager<ApplicationUser>();
        }
        [Fact]
        public void CustomerAdditionalPostReturnsView()
        {
            using var data = DatabaseMock.Instance;
        }

    }
}*/