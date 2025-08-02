using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core.Admin;
using EventAgency.Web.ViewModels.Admin.EventManagement;
using EventAgency.Web.ViewModels.Event;
using MockQueryable;
using Moq;

namespace EventAgency.Services.Tests
{
    [TestFixture]
    public class EventManagementServiceTests
    {
        private Mock<IEventRepository> eventRepositoryMock;
        private EventManagementService eventManagementService;

        [SetUp]
        public void SetUp()
        {
            this.eventRepositoryMock = new Mock<IEventRepository>();
            this.eventManagementService = new EventManagementService(this.eventRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        // Tests for GetAllEventsDataAsync method

        [Test]
        public async Task GetAllEventsDataAsync_ReturnsMappedEventsWithDefaultImageIfNull()
        {
            Guid eventId = Guid.NewGuid();

            List<Event> mockEvents = new List<Event>
            {
                new Event
                {
                    Id = eventId,
                    Name = "Birthday",
                    Description = "Fun event",
                    IsDeleted = false,
                    ImageUrl = null 
                }
            };

            IQueryable<Event> query = mockEvents.BuildMock();

            this.eventRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(query);

            IEnumerable<EventManagementIndexViewModel> result = await this.eventManagementService.GetAllEventsDataAsync();

            Assert.That(result.Count(), Is.EqualTo(1));
            EventManagementIndexViewModel ev = result.First();
            Assert.That(ev.Id, Is.EqualTo(eventId.ToString()));
            Assert.That(ev.ImageUrl, Is.EqualTo("/images/no-image.jpg")); 
        }

        [Test]
        public async Task GetAllEventsDataAsync_WithRealImageUrl_ReturnsIt()
        {
            Guid eventId = Guid.NewGuid();

            List<Event> mockEvents = new List<Event>
            {
                new Event
                {
                    Id = eventId,
                    Name = "Wedding",
                    Description = "Romantic",
                    IsDeleted = true,
                    ImageUrl = "wedding.jpg"
                }
            };

            IQueryable<Event> query = mockEvents.BuildMock();

            this.eventRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(query);

            IEnumerable<EventManagementIndexViewModel> result = await this.eventManagementService.GetAllEventsDataAsync();

            EventManagementIndexViewModel ev = result.First();
            Assert.That(ev.ImageUrl, Is.EqualTo("wedding.jpg"));
            Assert.That(ev.IsDeleted, Is.True);
        }

        [Test]
        public async Task GetAllEventsDataAsync_EmptyList_ReturnsEmpty()
        {
            List<Event> events = new List<Event>();
            IQueryable<Event> query = events.BuildMock();
            

            this.eventRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(query);

            IEnumerable<EventManagementIndexViewModel> result = await this.eventManagementService.GetAllEventsDataAsync();

            Assert.That(result, Is.Empty);
            Assert.IsNotNull(result, "Result should not be null even if empty");
        }

        // Tests for AddEventAsync method

        [Test]
        public async Task AddEventAsync_ValidInput_CreatesEventAndCallsAddAsync()
        {
            EventFormInputModel inputModel = new EventFormInputModel
            {
                Name = "Conference",
                Description = "Annual tech event",
                ImageUrl = "event.jpg"
            };

            Event? capturedEvent = null;

            this.eventRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Event>()))
                .Callback<Event>(e => capturedEvent = e)
                .Returns(Task.CompletedTask);

            await this.eventManagementService.AddEventAsync(inputModel);

            this.eventRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Event>()), Times.Once);
            Assert.That(capturedEvent, Is.Not.Null);
            Assert.That(capturedEvent.Name, Is.EqualTo("Conference"));
            Assert.That(capturedEvent.Description, Is.EqualTo("Annual tech event"));
            Assert.That(capturedEvent.ImageUrl, Is.EqualTo("event.jpg"));
        }

        // Tests for GetEditableEventByIdAsync method

        [Test]
        public async Task GetEditableEventByIdAsync_InvalidGuid_ReturnsNull()
        {
            EventFormInputModel? result = await this.eventManagementService.GetEditableEventByIdAsync("not-a-guid");

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetEditableEventByIdAsync_ValidIdButEventNotFound_ReturnsNull()
        {
            List<Event> events = new List<Event>();
            IQueryable<Event> emptyQuery = events.BuildMock();

            this.eventRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(emptyQuery);

            string validId = Guid.NewGuid().ToString();

            EventFormInputModel? result = await this.eventManagementService.GetEditableEventByIdAsync(validId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetEditableEventByIdAsync_EventFoundWithImage_ReturnsModel()
        {
            Guid eventId = Guid.NewGuid();
            List<Event> events = new List<Event>
            {
                new Event
                {
                    Id = eventId,
                    Name = "Concert",
                    Description = "Live music",
                    ImageUrl = "concert.jpg"
                }
            };

            IQueryable<Event> eventQuery = events.BuildMock();

            this.eventRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(eventQuery);

            EventFormInputModel? result = await this.eventManagementService.GetEditableEventByIdAsync(eventId.ToString());

            Assert.IsNotNull(result);
            Assert.That(result!.Name, Is.EqualTo("Concert"));
            Assert.That(result.ImageUrl, Is.EqualTo("concert.jpg"));
            Assert.That(result.Description, Is.EqualTo("Live music"));
        }

        [Test]
        public async Task GetEditableEventByIdAsync_EventFoundWithoutImage_ReturnsModelWithDefaultImage()
        {
            Guid eventId = Guid.NewGuid();
            List<Event> events = new List<Event>
            {
                new Event
                {
                    Id = eventId,
                    Name = "Concert",
                    Description = "Live music",
                    ImageUrl = null
                }
            };

            IQueryable<Event> eventQuery = events.BuildMock();

            this.eventRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(eventQuery);

            EventFormInputModel? result = await this.eventManagementService.GetEditableEventByIdAsync(eventId.ToString());

            Assert.IsNotNull(result);
            Assert.That(result!.ImageUrl, Is.EqualTo("/images/no-image.jpg"));
        }

        // Tests for EditEventAsync method


        [Test]
        public async Task EditEventAsync_InvalidGuid_ReturnsFalse()
        {
            EventFormInputModel input = new EventFormInputModel
            {
                Id = "not-a-guid",
                Name = "Test",
                Description = "Test",
                ImageUrl = "x.jpg"
            };

            bool result = await this.eventManagementService.EditEventAsync(input);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task EditEventAsync_EventNotFound_ReturnsFalse()
        {
            Guid id = Guid.NewGuid();

            this.eventRepositoryMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Event?)null);

            EventFormInputModel input = new EventFormInputModel
            {
                Id = id.ToString(),
                Name = "Updated",
                Description = "Updated desc",
                ImageUrl = "updated.jpg"
            };

            bool result = await this.eventManagementService.EditEventAsync(input);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task EditEventAsync_EventFoundAndUpdatedWithImageUrl_ReturnsTrue()
        {
            Guid id = Guid.NewGuid();

            Event existingEvent = new Event
            {
                Id = id,
                Name = "Old",
                Description = "Old Desc",
                ImageUrl = "old.jpg"
            };

            this.eventRepositoryMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(existingEvent);

            this.eventRepositoryMock
                .Setup(r => r.UpdateAsync(existingEvent))
                .ReturnsAsync(true);

            EventFormInputModel input = new EventFormInputModel
            {
                Id = id.ToString(),
                Name = "New Name",
                Description = "New Desc",
                ImageUrl = "new.jpg"
            };

            bool result = await this.eventManagementService.EditEventAsync(input);

            Assert.That(result, Is.True);
            Assert.That(existingEvent.Name, Is.EqualTo("New Name"));
            Assert.That(existingEvent.Description, Is.EqualTo("New Desc"));
            Assert.That(existingEvent.ImageUrl, Is.EqualTo("new.jpg"));
        }

        [Test]
        public async Task EditEventAsync_EventFoundWithNullImageUrl_SetsDefaultImageUrl()
        {
            Guid id = Guid.NewGuid();

            Event existingEvent = new Event
            {
                Id = id,
                Name = "Old",
                Description = "Old Desc",
                ImageUrl = "old.jpg"
            };

            this.eventRepositoryMock
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(existingEvent);

            this.eventRepositoryMock
                .Setup(r => r.UpdateAsync(existingEvent))
                .ReturnsAsync(true);

            EventFormInputModel input = new EventFormInputModel
            {
                Id = id.ToString(),
                Name = "Changed",
                Description = "Desc",
                ImageUrl = null
            };

            bool result = await this.eventManagementService.EditEventAsync(input);

            Assert.That(result, Is.True);
            Assert.That(existingEvent.ImageUrl, Is.EqualTo("/images/no-image.jpg")); // или какъвто е NoImageUrl
        }

        // Tests for DeleteOrRestoreEventAsync method

        [Test]
        public async Task DeleteOrRestoreEventAsync_NullOrEmptyId_ReturnsFalseTuple()
        {
            Tuple<bool, bool> result1 = await this.eventManagementService.DeleteOrRestoreEventAsync(null);
            Tuple<bool, bool> result2 = await this.eventManagementService.DeleteOrRestoreEventAsync("");

            Assert.IsFalse(result1.Item1);
            Assert.IsFalse(result1.Item2);
            Assert.IsFalse(result2.Item1);
            Assert.IsFalse(result2.Item2);
        }

        [Test]
        public async Task DeleteOrRestoreEventAsync_EventNotFound_ReturnsFalseTuple()
        {
            List<Event> events = new List<Event>();
            IQueryable<Event> emptyQuery = events.BuildMock();

            this.eventRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(emptyQuery);

            string id = Guid.NewGuid().ToString();
            Tuple<bool, bool> result = await this.eventManagementService.DeleteOrRestoreEventAsync(id);

            Assert.IsFalse(result.Item1);
            Assert.IsFalse(result.Item2);
        }

        [Test]
        public async Task DeleteOrRestoreEventAsync_EventFoundAndActive_MarksAsDeletedAndReturnsTrueFalse()
        {
            Guid eventId = Guid.NewGuid();
            List<Event> events = new List<Event>
            {
                new Event
                {
                    Id = eventId,
                    Name = "Some Event",
                    IsDeleted = false
                }
            };

            IQueryable<Event> query = events.BuildMock();

            this.eventRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(query);

            this.eventRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Event>()))
                .ReturnsAsync(true);

            Tuple<bool, bool> result = await this.eventManagementService.DeleteOrRestoreEventAsync(eventId.ToString());

            Assert.IsTrue(result.Item1);
            Assert.IsFalse(result.Item2);
            Assert.IsTrue(events.First().IsDeleted);
        }

        [Test]
        public async Task DeleteOrRestoreEventAsync_EventFoundAndDeleted_RestoresAndReturnsTrueTrue()
        {
            Guid eventId = Guid.NewGuid();
            List<Event> events = new List<Event>
            {
                new Event
                {
                    Id = eventId,
                    Name = "Archived Event",
                    IsDeleted = true
                }
            };

            IQueryable<Event> query = events.BuildMock();

            this.eventRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(query);

            this.eventRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Event>()))
                .ReturnsAsync(true);

            Tuple<bool, bool> result = await this.eventManagementService.DeleteOrRestoreEventAsync(eventId.ToString());

            Assert.IsTrue(result.Item1);
            Assert.IsTrue(result.Item2);
            Assert.IsFalse(events.First().IsDeleted);
        }



    }
}
