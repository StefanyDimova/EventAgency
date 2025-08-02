using EventAgency.Services.Core.Admin;
using EventAgency.Web.ViewModels.Admin.UserManagement;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using Moq;
using Moq.AutoMock;

namespace EventAgency.Services.Tests
{
    [TestFixture]
    public class UserServiceAdminTests
    {
        private AutoMocker mocker;
        private UserService userService;

        [SetUp]
        public void Setup()
        {
            this.mocker = new AutoMocker();
            this.userService = new UserService(
                this.mocker.GetMock<UserManager<IdentityUser>>().Object,
                this.mocker.GetMock<RoleManager<IdentityRole>>().Object
            );
        }

        // Tests for UserExistsByIdAsync method

        [Test]
        public async Task UserExistsByIdAsync_ShouldReturnTrue_WhenUserExists()
        {
            Guid testUserId = Guid.NewGuid();
            var user = new IdentityUser { Id = testUserId.ToString() };

            this.mocker.GetMock<UserManager<IdentityUser>>()
                .Setup(m => m.FindByIdAsync(testUserId.ToString()))
                .ReturnsAsync(user);

            bool result = await this.userService.UserExistsByIdAsync(testUserId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task UserExistsByIdAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            Guid testUserId = Guid.NewGuid();

            this.mocker.GetMock<UserManager<IdentityUser>>()
                .Setup(m => m.FindByIdAsync(testUserId.ToString()))
                .ReturnsAsync((IdentityUser?)null);

            bool result = await this.userService.UserExistsByIdAsync(testUserId);

            Assert.IsFalse(result);
        }

        // Tests for AssignUserToRoleAsync method

        [Test]
        public void AssignUserToRoleAsync_ShouldThrowException_WhenUserNotFound()
        {
            RoleSelectionInputModel model = new RoleSelectionInputModel
            {
                UserId = "123",
                Role = "Admin"
            };

            Mock<UserManager<IdentityUser>> userManagerMock = this.mocker.GetMock<UserManager<IdentityUser>>();

            userManagerMock
                .Setup(m => m.FindByIdAsync(model.UserId))
                .ReturnsAsync((IdentityUser)null!);

            ArgumentException? ex = Assert.ThrowsAsync<ArgumentException>(() =>
                this.userService.AssignUserToRoleAsync(model));

            Assert.That(ex?.Message, Is.EqualTo("User does not exist!"));
        }

        [Test]
        public void AssignUserToRoleAsync_ShouldThrowException_WhenRoleIsInvalid()
        {
            RoleSelectionInputModel model = new RoleSelectionInputModel
            {
                UserId = "123",
                Role = "FakeRole"
            };

            IdentityUser user = new IdentityUser();

            Mock<UserManager<IdentityUser>> userManagerMock = this.mocker.GetMock<UserManager<IdentityUser>>();

            userManagerMock
                .Setup(m => m.FindByIdAsync(model.UserId))
                .ReturnsAsync(user);

            Mock<RoleManager<IdentityRole>> roleManagerMock = this.mocker.GetMock<RoleManager<IdentityRole>>();

            roleManagerMock
                .Setup(r => r.RoleExistsAsync(model.Role))
                .ReturnsAsync(false);

            ArgumentException? ex = Assert.ThrowsAsync<ArgumentException>(() =>
                this.userService.AssignUserToRoleAsync(model));

            Assert.That(ex?.Message, Is.EqualTo("Selected role is not a valid role!"));
        }

        [Test]
        public async Task AssignUserToRoleAsync_ShouldReturnTrue_WhenValid()
        {
            RoleSelectionInputModel model = new RoleSelectionInputModel
            {
                UserId = "123",
                Role = "Admin"
            };

            IdentityUser user = new IdentityUser();

            Mock<UserManager<IdentityUser>> userManagerMock = this.mocker.GetMock<UserManager<IdentityUser>>();

            userManagerMock
                .Setup(m => m.FindByIdAsync(model.UserId))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(m => m.AddToRoleAsync(user, model.Role))
                .ReturnsAsync(IdentityResult.Success);

            Mock<RoleManager<IdentityRole>> roleManagerMock = this.mocker.GetMock<RoleManager<IdentityRole>>();

            roleManagerMock
                .Setup(r => r.RoleExistsAsync(model.Role))
                .ReturnsAsync(true);

            bool result = await this.userService.AssignUserToRoleAsync(model);

            Assert.IsTrue(result);
        }

        [Test]
        public void AssignUserToRoleAsync_ShouldThrowException_WhenAddToRoleFails()
        {
            RoleSelectionInputModel model = new RoleSelectionInputModel
            {
                UserId = "123",
                Role = "Admin"
            };

            IdentityUser user = new IdentityUser();

            Mock<UserManager<IdentityUser>> userManagerMock = this.mocker.GetMock<UserManager<IdentityUser>>();

            userManagerMock
                .Setup(m => m.FindByIdAsync(model.UserId))
                .ReturnsAsync(user);

            userManagerMock
                .Setup(m => m.AddToRoleAsync(user, model.Role))
                .ThrowsAsync(new Exception("Unexpected DB error"));

            Mock<RoleManager<IdentityRole>> roleManagerMock = this.mocker.GetMock<RoleManager<IdentityRole>>();

            roleManagerMock
                .Setup(r => r.RoleExistsAsync(model.Role))
                .ReturnsAsync(true);

            ArgumentException? ex = Assert.ThrowsAsync<ArgumentException>(() =>
                this.userService.AssignUserToRoleAsync(model));

            Assert.That(ex?.Message, Does.StartWith("Unexpected error occurred while adding the user to role"));
        }

        // Tests for GetAllUsersAsync method

        [Test]
        public async Task GetAllUsersAsync_ShouldReturnUsersExceptCurrent_WithRoles()
        {
            string currentUserId = "current-user-id";

            IdentityUser user1 = new IdentityUser
            {
                Id = "user-1",
                Email = "user1@example.com"
            };

            IdentityUser user2 = new IdentityUser
            {
                Id = currentUserId,
                Email = "user2@example.com"
            };

            List<IdentityUser> usersList = new List<IdentityUser> { user1, user2 };

            Mock<UserManager<IdentityUser>> userManagerMock = this.mocker.GetMock<UserManager<IdentityUser>>();
            userManagerMock
                .Setup(u => u.Users)
                .Returns(usersList.BuildMock());

            userManagerMock
                .Setup(u => u.GetRolesAsync(It.Is<IdentityUser>(usr => usr.Id == "user-1")))
                .ReturnsAsync(new List<string> { "Admin", "User" });

            IEnumerable<UserManagementIndexViewModel> result =
                await this.userService.GetAllUsersAsync(currentUserId);

            List<UserManagementIndexViewModel> resultList = result.ToList();

            Assert.AreEqual(1, resultList.Count);
            Assert.AreEqual("user-1", resultList.First().Id);
            Assert.AreEqual("user1@example.com", resultList.First().Email);
            CollectionAssert.AreEquivalent(new[] { "Admin", "User" }, resultList[0].Roles);
        }

        // Tests for RemoveUserRoleAsync method

        [Test]
        public async Task RemoveUserRoleAsync_ShouldReturnFalse_WhenUserNotFound()
        {
            Guid userId = Guid.NewGuid();
            string roleName = "Admin";

            Mock<UserManager<IdentityUser>> userManagerMock = this.mocker.GetMock<UserManager<IdentityUser>>();

            userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString())).ReturnsAsync((IdentityUser?)null);

            Mock<RoleManager<IdentityRole>> roleManagerMock = this.mocker.GetMock<RoleManager<IdentityRole>>();

            roleManagerMock.Setup(r => r.RoleExistsAsync(roleName)).ReturnsAsync(true);

            bool result = await this.userService.RemoveUserRoleAsync(userId, roleName);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveUserRoleAsync_ShouldReturnFalse_WhenRoleDoesNotExist()
        {
            Guid userId = Guid.NewGuid();
            string roleName = "Manager";

            IdentityUser user = new IdentityUser { Id = userId.ToString() };

            Mock<UserManager<IdentityUser>> userManagerMock = this.mocker.GetMock<UserManager<IdentityUser>>();
            userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString())).ReturnsAsync(user);

            Mock<RoleManager<IdentityRole>> roleManagerMock = this.mocker.GetMock<RoleManager<IdentityRole>>();
            roleManagerMock.Setup(r => r.RoleExistsAsync(roleName)).ReturnsAsync(false);

            bool result = await this.userService.RemoveUserRoleAsync(userId, roleName);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveUserRoleAsync_ShouldReturnTrue_WhenUserNotInRole()
        {
            Guid userId = Guid.NewGuid();
            string roleName = "User";

            IdentityUser user = new IdentityUser { Id = userId.ToString() };

            Mock<UserManager<IdentityUser>> userManagerMock = this.mocker.GetMock<UserManager<IdentityUser>>();
            userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            userManagerMock.Setup(u => u.IsInRoleAsync(user, roleName)).ReturnsAsync(false);

            Mock<RoleManager<IdentityRole>> roleManagerMock = this.mocker.GetMock<RoleManager<IdentityRole>>();
            roleManagerMock.Setup(r => r.RoleExistsAsync(roleName)).ReturnsAsync(true);

            bool result = await this.userService.RemoveUserRoleAsync(userId, roleName);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task RemoveUserRoleAsync_ShouldReturnTrue_WhenUserInRole_AndRemovalSucceeds()
        {
            Guid userId = Guid.NewGuid();
            string roleName = "Admin";

            IdentityUser user = new IdentityUser { Id = userId.ToString() };

            Mock<UserManager<IdentityUser>> userManagerMock = this.mocker.GetMock<UserManager<IdentityUser>>();
            userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            userManagerMock.Setup(u => u.IsInRoleAsync(user, roleName)).ReturnsAsync(true);
            userManagerMock.Setup(u => u.RemoveFromRoleAsync(user, roleName)).ReturnsAsync(IdentityResult.Success);

            Mock<RoleManager<IdentityRole>> roleManagerMock = this.mocker.GetMock<RoleManager<IdentityRole>>();
            roleManagerMock.Setup(r => r.RoleExistsAsync(roleName)).ReturnsAsync(true);

            bool result = await this.userService.RemoveUserRoleAsync(userId, roleName);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task RemoveUserRoleAsync_ShouldReturnFalse_WhenRemovalFails()
        {
            Guid userId = Guid.NewGuid();
            string roleName = "Admin";

            IdentityUser user = new IdentityUser { Id = userId.ToString() };

            IdentityError error = new IdentityError { Description = "Error" };
            IdentityResult failedResult = IdentityResult.Failed(error);

            Mock<UserManager<IdentityUser>> userManagerMock = this.mocker.GetMock<UserManager<IdentityUser>>();
            userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            userManagerMock.Setup(u => u.IsInRoleAsync(user, roleName)).ReturnsAsync(true);
            userManagerMock.Setup(u => u.RemoveFromRoleAsync(user, roleName)).ReturnsAsync(failedResult);

            Mock<RoleManager<IdentityRole>> roleManagerMock = this.mocker.GetMock<RoleManager<IdentityRole>>();
            roleManagerMock.Setup(r => r.RoleExistsAsync(roleName)).ReturnsAsync(true);

            bool result = await this.userService.RemoveUserRoleAsync(userId, roleName);

            Assert.IsFalse(result);
        }

        // Tests for DeleteUserAsync method

        [Test]
        public async Task DeleteUserAsync_ShouldReturnFalse_WhenUserNotFound()
        {
            Guid userId = Guid.NewGuid();
            this.mocker.GetMock<UserManager<IdentityUser>>()
                .Setup(um => um.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((IdentityUser?)null);

            bool result = await this.userService.DeleteUserAsync(userId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteUserAsync_ShouldReturnFalse_WhenDeletionFails()
        {
            Guid userId = Guid.NewGuid();
            IdentityUser mockUser = new IdentityUser { Id = userId.ToString() };

            this.mocker.GetMock<UserManager<IdentityUser>>()
                .Setup(um => um.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(mockUser);

            this.mocker.GetMock<UserManager<IdentityUser>>()
                .Setup(um => um.DeleteAsync(mockUser))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed" }));

            bool result = await this.userService.DeleteUserAsync(userId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteUserAsync_ShouldReturnTrue_WhenUserIsDeletedSuccessfully()
        {
            Guid userId = Guid.NewGuid();
            IdentityUser mockUser = new IdentityUser { Id = userId.ToString() };

            this.mocker.GetMock<UserManager<IdentityUser>>()
                .Setup(um => um.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(mockUser);

            this.mocker.GetMock<UserManager<IdentityUser>>()
                .Setup(um => um.DeleteAsync(mockUser))
                .ReturnsAsync(IdentityResult.Success);

            bool result = await this.userService.DeleteUserAsync(userId);

            Assert.IsTrue(result);
        }

    }
}
    