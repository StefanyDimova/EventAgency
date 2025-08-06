using EventAgency.Services.Core.Interfaces;
using EventAgency.Web.Controllers;
using EventAgency.Web.ViewModels.Event;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventAgency.Services.Tests.Controllers
{
    [TestFixture]
    public class EventControllerTests
    {
        private Mock<IEventService> mockEventService;
        private EventController controller;

        [SetUp]
        public void SetUp()
        {
            this.mockEventService = new Mock<IEventService>();
            this.controller = new EventController(this.mockEventService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            this.controller?.Dispose();
        }

        [Test]
        public async Task Index_ShouldReturnViewWithEvents_WhenNoExceptionIsThrown()
        {
            List<AllEventsViewModel> testEvents = new List<AllEventsViewModel>
            {
                new AllEventsViewModel { Id = "1", Name = "Test Event 1" },
                new AllEventsViewModel { Id = "2", Name = "Test Event 2" }
            };

            this.mockEventService.Setup(x => x.GetAllEventsAsync())
                .ReturnsAsync(testEvents);

            IActionResult result = await this.controller.Index();

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOf<IEnumerable<AllEventsViewModel>>(viewResult.Model);
        }

        [Test]
        public async Task Index_ShouldRedirectToError_WhenExceptionIsThrown()
        {
            this.mockEventService.Setup(x => x.GetAllEventsAsync())
                .ThrowsAsync(new Exception());

            IActionResult result = await this.controller.Index();

            // Assert
            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Error", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
            Assert.AreEqual(500, redirectResult.RouteValues["statusCode"]);
        }

        [Test]
        public async Task Details_ShouldReturnViewWithEvent_WhenEventExists()
        {
            string eventId = "abc123";

            DetailsEventViewModel testEvent = new DetailsEventViewModel
            {
                Id = eventId,
                Name = "Test Event"
            };

            this.mockEventService.Setup(x => x.GetEventDetailsByIdAsync(eventId))
                .ReturnsAsync(testEvent);

            IActionResult result = await this.controller.Details(eventId);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsInstanceOf<DetailsEventViewModel>(viewResult.Model);
        }

        [Test]
        public async Task Details_ShouldRedirectToNotFound_WhenEventDoesNotExist()
        {
            // Arrange
            string eventId = "notfound";

            this.mockEventService.Setup(x => x.GetEventDetailsByIdAsync(eventId))
                .ReturnsAsync((DetailsEventViewModel)null);

            // Act
            IActionResult result = await this.controller.Details(eventId);

            // Assert
            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Error", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
            Assert.AreEqual(404, redirectResult.RouteValues["statusCode"]);
        }

        [Test]
        public async Task Details_ShouldRedirectToServerError_WhenExceptionIsThrown()
        {
            // Arrange
            string eventId = "abc123";

            this.mockEventService.Setup(x => x.GetEventDetailsByIdAsync(eventId))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            IActionResult result = await this.controller.Details(eventId);

            // Assert
            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Error", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
            Assert.AreEqual(500, redirectResult.RouteValues["statusCode"]);
        }


    }
}
