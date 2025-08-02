using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Admin;
using EventAgency.Web.ViewModels.Admin.CategoryManagement;
using Microsoft.AspNetCore.Mvc.Rendering;
using MockQueryable;
using Moq;

namespace EventAgency.Services.Tests
{
    [TestFixture]
    public class CategoryManagementServiceTests
    {
        private Mock<ICategoryRepository> categoryRepoMock;
        private CategoryManagementService categoryService;

        [SetUp]
        public void SetUp()
        {
            this.categoryRepoMock = new Mock<ICategoryRepository>();
            this.categoryService = new CategoryManagementService(this.categoryRepoMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        // Tests for GetAllCategoriesAsync method

        [Test]
        public async Task GetAllCategoriesAsync_WithSubCategories_ReturnsMappedViewModels()
        {
            List<Category> categories = new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Main Category",
                    ParentCategoryId = null,
                    SubCategories = new List<Category>
                    {
                        new Category { Id = 2, Name = "Sub 1" },
                        new Category { Id = 3, Name = "Sub 2" }
                    }
                }
            };
            IQueryable<Category> categoryQuery = categories.BuildMock();

            this.categoryRepoMock
                .Setup(r => r.GetAllAttached())
                .Returns(categoryQuery);

            IEnumerable<CategoryManagementViewModel> result = await this.categoryService.GetAllCategoriesAsync();

            Assert.That(result.Count(), Is.EqualTo(1));

            CategoryManagementViewModel main = result.First();

            Assert.That(main.Name, Is.EqualTo("Main Category"));
            Assert.That(main.SubCategories.Count, Is.EqualTo(2));
            Assert.That(main.SubCategories.Any(sc => sc.Name == "Sub 1"), Is.True);
            Assert.That(main.SubCategories.Any(sc => sc.Name == "Sub 2"), Is.True);
        }

        [Test]
        public async Task GetAllCategoriesAsync_EmptyList_ReturnsEmpty()
        {
            List<Category> categories = new List<Category>();
            IQueryable<Category> emptyQuery = categories.BuildMock();

            this.categoryRepoMock
                .Setup(r => r.GetAllAttached())
                .Returns(emptyQuery);

            IEnumerable<CategoryManagementViewModel> result = await this.categoryService.GetAllCategoriesAsync();

            Assert.IsEmpty(result);
            Assert.IsNotNull(result, "Result should not be null even if empty");
        }

        // Tests for GetCategorySelectListAsync method


        [Test]
        public async Task GetCategorySelectListAsync_WithRootCategories_ReturnsSelectListItems()
        {
            List<Category> categories = new List<Category>
            {
                new Category { Id = 1, Name = "Weddings", ParentCategoryId = null },
                new Category { Id = 2, Name = "Birthdays", ParentCategoryId = null },
                new Category { Id = 3, Name = "Child", ParentCategoryId = 1 } 
            };

            IQueryable<Category> query = categories.BuildMock();

            this.categoryRepoMock
                .Setup(r => r.GetAllAttached())
                .Returns(query);

            IEnumerable<SelectListItem> result = await this.categoryService.GetCategorySelectListAsync();

            List<SelectListItem> resultList = result.ToList();

            Assert.That(resultList.Count, Is.EqualTo(2));
            Assert.That(resultList.Any(c => c.Text == "Weddings" && c.Value == "1"), Is.True);
            Assert.That(resultList.Any(c => c.Text == "Birthdays" && c.Value == "2"), Is.True);
            Assert.That(resultList.Any(c => c.Text == "Child"), Is.False);
        }

        [Test]
        public async Task GetCategorySelectListAsync_Empty_ReturnsEmpty()
        {
            List<Category> emptyCategories = new List<Category>();
            IQueryable<Category> emptyQuery = emptyCategories.BuildMock();

            this.categoryRepoMock
                .Setup(r => r.GetAllAttached())
                .Returns(emptyQuery);

            IEnumerable<SelectListItem> result = await this.categoryService.GetCategorySelectListAsync();

            Assert.IsEmpty(result);
            Assert.IsNotNull(result, "Result should not be null even if empty");
        }

        // Tests for CreateCategoryAsync method

        [Test]
        public async Task CreateCategoryAsync_ValidModel_AddsCategory()
        {
            CreateCategoryManagementFormModel model = new CreateCategoryManagementFormModel
            {
                Name = "Decorations",
                ParentCategoryId = null
            };

            Category? capturedCategory = null;

            this.categoryRepoMock
                .Setup(r => r.AddAsync(It.IsAny<Category>()))
                .Callback<Category>(c => capturedCategory = c)
                .Returns(Task.CompletedTask);

            await this.categoryService.CreateCategoryAsync(model);

            Assert.IsNotNull(capturedCategory);
            Assert.That(capturedCategory!.Name, Is.EqualTo("Decorations"));
            Assert.IsNull(capturedCategory.ParentCategoryId);

            this.categoryRepoMock.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
        }
    }
}
