using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core;
using EventAgency.Web.ViewModels.Admin.ProductManagement;
using EventAgency.Web.ViewModels.Category;
using Microsoft.AspNetCore.Mvc.Rendering;
using MockQueryable;
using Moq;

namespace EventAgency.Services.Tests
{
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> categoryRepositoryMock;
        private CategoryService categoryService;

        [SetUp]
        public void Setup()
        {
            this.categoryRepositoryMock = new Mock<ICategoryRepository>();
            this.categoryService = new CategoryService(this.categoryRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        // Tests for GetAllCategoriesAsync method

        [Test]
        public async Task GetAllCategoriesAsync_ShouldReturnEmptyCollection_WhenNoCategoriesExist()
        {
            List<Category> categories = new List<Category>();

            IQueryable<Category> categoriesQueryable = categories.BuildMock();
            this.categoryRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(categoriesQueryable);

            IEnumerable<CategoryViewModel> emptyViewModelCollection = await this.categoryService.GetAllCategoriesAsync();

            Assert.IsNotNull(emptyViewModelCollection);
            Assert.AreEqual(categories.Count(), emptyViewModelCollection.Count());
        }

        [Test]
        public async Task GetAllCategoriesAsync_ShouldReturnMappedCategoriesWithSubcategories()
        {
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Test Category",
                    SubCategories = new List<Category>()
                    {
                        new Category() { Id = 1, Name = "Test Subcategory 1" },
                        new Category() { Id = 2, Name = "Test Subcategory 2" }
                    }
                }
            };
            IQueryable<Category> categoriesQueryable = categories.BuildMock();

            this.categoryRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(categoriesQueryable);

            IEnumerable<CategoryViewModel> viewModelCollection = await this.categoryService.GetAllCategoriesAsync();

            Assert.IsNotNull(viewModelCollection);
            Assert.AreEqual(1, viewModelCollection.Count());

            Assert.AreEqual("Test Category", viewModelCollection.First().Name);
            Assert.AreEqual(2, viewModelCollection.First().SubCategories.Count);
        }

        [Test]
        public async Task GetAllCategoriesAsync_ShouldReturnCategoryWithoutSubcategories()
        {
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Test Category",
                    SubCategories = new List<Category>()
                }
            };

            IQueryable<Category> categoriesQueryable = categories.BuildMock();
            this.categoryRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(categoriesQueryable);

            IEnumerable<CategoryViewModel> viewModelCollection = await this.categoryService.GetAllCategoriesAsync();
            Assert.IsNotNull(viewModelCollection);
            Assert.AreEqual(1, viewModelCollection.Count());
            Assert.AreEqual("Test Category", viewModelCollection.First().Name);
        }

        [Test]
        public async Task GetAllCategoriesAsync_ShouldFilterOutNonMainCategories()
        {
            List<Category> categories = new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Test Category",
                    ParentCategoryId = 3,
                    SubCategories = new List<Category>()
                }
            };

            IQueryable<Category> categoriesQueryable = categories.BuildMock();
            this.categoryRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(categoriesQueryable);

            IEnumerable<CategoryViewModel> viewModelCollection = await this.categoryService.GetAllCategoriesAsync();
            Assert.IsEmpty(viewModelCollection);

        }

        // Tests for GetCategorySelectListAsync method

        [Test]
        public async Task GetCategorySelectListAsync_ShouldReturnCorrectSelectListItems()
        {
            List<Category> categories = new List<Category>()
            {
                new Category() { Id = 1, Name = "Category 1", ParentCategoryId = null },
                new Category() { Id = 2, Name = "Category 2", ParentCategoryId = null }
            };

            IQueryable<Category> categoriesQueryable = categories.BuildMock();

            this.categoryRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(categoriesQueryable);

            IEnumerable<SelectListItem> selectListItems = await this.categoryService.GetCategorySelectListAsync();

            Assert.IsNotNull(selectListItems);
            Assert.AreEqual(2, selectListItems.Count());
        }

        [Test]
        public async Task GetCategorySelectListAsync_ShouldFilterOutSubcategories()
        {
            List<Category> categories = new List<Category>()
            {
                new Category() { Id = 1, Name = "Main", ParentCategoryId = null },
                new Category() { Id = 2, Name = "Sub", ParentCategoryId = 1 }
            };


            IQueryable<Category> categoriesQueryable = categories.BuildMock();

            this.categoryRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(categoriesQueryable);

            IEnumerable<SelectListItem> selectListItems = await this.categoryService.GetCategorySelectListAsync();

            Assert.AreEqual(1, selectListItems.Count());
            Assert.AreEqual("Main", selectListItems.First().Text);
        }

        [Test]
        public async Task GetCategorySelectListAsync_ShouldReturnEmpty_WhenNoMainCategoriesExist()
        {
            List<Category> categories = new List<Category>();

            IQueryable<Category> categoriesQueryable = categories.BuildMock();

            this.categoryRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(categoriesQueryable);

            IEnumerable<SelectListItem> selectListItems = await this.categoryService.GetCategorySelectListAsync();

            Assert.IsNotNull(selectListItems);
            Assert.IsEmpty(selectListItems);
        }

        // Tests for GetCategoriesDropdownDataAsync method

        [Test]
        public async Task GetCategoriesDropdownDataAsync_ShouldReturnMainCategories_AsDropDownModels()
        {
            List<Category> categories = new List<Category>()
            {
                new Category() { Id = 1, Name = "Category 1", ParentCategoryId = null },
                new Category() { Id = 2, Name = "Category 2", ParentCategoryId = null }
            };

            IQueryable<Category> categoriesQueryable = categories.BuildMock();

            this.categoryRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(categoriesQueryable);

            IEnumerable<AddProductCategoryDropDownModel> dropdownData = await this.categoryService.GetCategoriesDropdownDataAsync();

            Assert.IsNotNull(dropdownData);
            Assert.AreEqual(2, dropdownData.Count());

        }

        [Test]
        public async Task GetCategoriesDropdownDataAsync_ShouldExcludeSubcategories()
        {
            List<Category> categories = new List<Category>()
            {
                new Category() { Id = 1, Name = "Main Category", ParentCategoryId = null },
                new Category() { Id = 2, Name = "Sub Category", ParentCategoryId = 1 }
            };


            IQueryable<Category> categoriesQueryable = categories.BuildMock();
            this.categoryRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(categoriesQueryable);

            IEnumerable<AddProductCategoryDropDownModel> dropdownData = await this.categoryService.GetCategoriesDropdownDataAsync();

            Assert.AreEqual(1, dropdownData.Count());
            Assert.AreEqual("Main Category", dropdownData.First().Name);
        }

        [Test]
        public async Task GetCategoriesDropdownDataAsync_ShouldReturnEmptyList_WhenNoMainCategoriesExist()
        {
            List<Category> categories = new List<Category>();

            IQueryable<Category> categoriesQueryable = categories.BuildMock();
            this.categoryRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(categoriesQueryable);

            IEnumerable<AddProductCategoryDropDownModel> dropdownData = await this.categoryService.GetCategoriesDropdownDataAsync();

            Assert.IsNotNull(dropdownData);
            Assert.IsEmpty(dropdownData);
        }

        // Tests for GetSubCategoriesDropdownDataAsync method

        [Test]
        public async Task GetSubCategoriesDropdownDataAsync_ShouldReturnCorrectSubcategories()
        {
            int parentCategoryId = 1;
            List<Category> categories = new List<Category>()
            {
                new Category() { Id = 2, Name = "Sub 1", ParentCategoryId = 1 },
                new Category() { Id = 3, Name = "Sub 2", ParentCategoryId = 1 },
                new Category() { Id = 4, Name = "Other Sub", ParentCategoryId = 20 }
            };

            IQueryable<Category> categoriesQueryable = categories.BuildMock();
            this.categoryRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(categoriesQueryable);

            IEnumerable<AddProductCategoryDropDownModel> dropdownData = await this.categoryService.GetSubCategoriesDropdownDataAsync(parentCategoryId);

            Assert.AreEqual(2, dropdownData.Count());
            Assert.AreEqual("Sub 1", dropdownData.First().Name);
            Assert.IsTrue(dropdownData.All(r => r.Id == 2 || r.Id == 3));
        }

        [Test]
        public async Task GetSubCategoriesDropdownDataAsync_ShouldReturnEmpty_WhenNoMatchingParent()
        {
            int parentCategoryId = 1;
            List<Category> categories = new List<Category>()
            {
                new Category() { Id = 2, Name = "Sub 1", ParentCategoryId = null },
                new Category() { Id = 3, Name = "Sub 2", ParentCategoryId = 2 },
                new Category() { Id = 4, Name = "Other Sub", ParentCategoryId = 20 }
            };

            IQueryable<Category> categoriesQueryable = categories.BuildMock();
            this.categoryRepositoryMock
                .Setup(cr => cr.GetAllAttached())
                .Returns(categoriesQueryable);

            IEnumerable<AddProductCategoryDropDownModel> dropdownData = await this.categoryService.GetSubCategoriesDropdownDataAsync(parentCategoryId);

            Assert.IsNotNull(dropdownData);
            Assert.IsEmpty(dropdownData);
        }

    }
}
