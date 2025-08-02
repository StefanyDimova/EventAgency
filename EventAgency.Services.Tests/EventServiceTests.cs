using EventAgency.Data.Models;
using EventAgency.Data.Repository.Interfaces;
using EventAgency.Services.Core;
using EventAgency.Web.ViewModels.Event;
using MockQueryable;
using Moq;
using NUnit.Framework;

namespace EventAgency.Services.Tests
{
    [TestFixture]
    public class EventServiceTests
    {
        private Mock<IEventRepository> eventRepositoryMock;
        private EventService eventService;

        [SetUp]
        public void Setup()
        {
            this.eventRepositoryMock = new Mock<IEventRepository>();
            this.eventService = new EventService(this.eventRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        // Tests for GetAllEventsAsync method

        [Test]
        public async Task GetAllEventsAsync_ShouldReturnEmptyCollection_WhenNoEventsExist()
        {
            List<Event> events = new List<Event>();
            IQueryable<Event> eventsQueryable = events.BuildMock();

            this.eventRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(eventsQueryable);

            IEnumerable<AllEventsViewModel> emptyViewModelCollection = await this.eventService.GetAllEventsAsync();

            Assert.IsNotNull(emptyViewModelCollection);
            Assert.AreEqual(events.Count(), emptyViewModelCollection.Count());
        }

        [Test]

        public async Task GetAllEventsAsync_ShouldReturnCorrectViewModel_WhenEventsExist()
        {
            List<Event> events = new List<Event>()
            {
                new Event()
                {
                    Id = Guid.Parse("44ced817-d057-4285-ad16-ad2f3ecafbd6"),
                    Name = "Test Event 1",
                    Description = "This is a test event",
                    ImageUrl = "https://example.com/image1.jpg",
                    IsDeleted = false
                },
                new Event()
                {
                    Id = Guid.Parse("40ed0b74-49b2-4c9d-abf0-c59931bace04"),
                    Name = "Test Event 2",
                    Description = "This is another test event",
                    ImageUrl = "https://example.com/image2.jpg",
                    IsDeleted = false
                }
            };
            IQueryable<Event> eventsQueryable = events.BuildMock();

            this.eventRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(eventsQueryable);

            IEnumerable<AllEventsViewModel> viewModelCollection = await this.eventService.GetAllEventsAsync();

            Assert.IsNotNull(viewModelCollection);
            Assert.AreEqual(events.Count(), viewModelCollection.Count());
        }

        [Test]
        public async Task GetAllEventsView_ShouldReturnSameDataInViewModels()
        {
            List<Event> events = new List<Event>()
            {
                new Event()
                {
                    Id = Guid.Parse("44ced817-d057-4285-ad16-ad2f3ecafbd6"),
                    Name = "Test Event 1",
                    Description = "This is a test event",
                    ImageUrl = "https://example.com/image1.jpg",
                    IsDeleted = false
                },
                new Event()
                {
                    Id = Guid.Parse("40ed0b74-49b2-4c9d-abf0-c59931bace04"),
                    Name = "Test Event 2",
                    Description = "This is another test event",
                    ImageUrl = "https://example.com/image2.jpg",
                    IsDeleted = false
                }
            };
            IQueryable<Event> eventsQueryable = events.BuildMock();

            this.eventRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(eventsQueryable);

            IEnumerable<AllEventsViewModel> viewModelCollection = await this.eventService.GetAllEventsAsync();

            Assert.IsNotNull(viewModelCollection);
            Assert.AreEqual(events.Count(), viewModelCollection.Count());

            foreach (Event newEvent in events)
            {
                AllEventsViewModel? eventsViewModel = viewModelCollection
                    .FirstOrDefault(vm => vm.Id.ToLower() == newEvent.Id.ToString().ToLower());

                Assert.IsNotNull(eventsViewModel);
                Assert.AreEqual(newEvent.Name, eventsViewModel.Name, "Event name does not match between repository and ViewModel");
                Assert.AreEqual(newEvent.Description, eventsViewModel.Description);

            }
        }

        [Test]
        public async Task GetAllEventsAsync_ShouldReplaceNullImageUrl_WithDefault()
        {
            List<Event> events = new List<Event>()
            {
                new Event()
                {
                    Id = Guid.Parse("44ced817-d057-4285-ad16-ad2f3ecafbd6"),
                    Name = "Test Event 1",
                    Description = "This is a test event",
                    ImageUrl = null,
                    IsDeleted = false
                }
            };

            IQueryable<Event> eventsQueryable = events.BuildMock();

            this.eventRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(eventsQueryable);

            IEnumerable<AllEventsViewModel> viewModelCollection = await this.eventService.GetAllEventsAsync();

            AllEventsViewModel vm = viewModelCollection.First();

            Assert.AreEqual(events.Count(), viewModelCollection.Count());
            Assert.AreEqual("/images/no-image.jpg", vm.ImageUrl);
        }

        // Tests for GetEventDetailsByIdAsync method

        [Test]
        public async Task GetEventDetailsByIdAsync_ShouldReturnNull_WhenIdIsNull()
        {
            DetailsEventViewModel? detailsEventViewModel = await this.eventService.GetEventDetailsByIdAsync(null);
            Assert.IsNull(detailsEventViewModel, "Expected null when id is null");
        }

        [Test]
        public async Task GetEventDetailsByIdAsync_ShouldReturnNull_WhenIdIsInvalidGuid()
        {
            DetailsEventViewModel? detailsEventViewModel = await eventService.GetEventDetailsByIdAsync("invalid-guid");
            Assert.IsNull(detailsEventViewModel);
        }

        [Test]
        public async Task GetEventDetailsByIdAsync_ShouldReturnNull_WhenEventNotFound()
        {
            List<Event> emptyEvents = new List<Event>();
            IQueryable<Event> eventsQueryable = emptyEvents.BuildMock();


            this.eventRepositoryMock.Setup(r => r.GetAllAttached()).Returns(eventsQueryable);

            DetailsEventViewModel? detailsEventViewModel = await eventService.GetEventDetailsByIdAsync(Guid.NewGuid().ToString());

            Assert.IsNull(detailsEventViewModel);
        }

        [Test]
        public async Task GetEventDetailsByIdAsync_ShouldReturnEvent_WhenEventFound_WithValidImage()
        {
            List<Event> events = new List<Event>()
            {
                new Event()
                {
                    Id = Guid.Parse("44ced817-d057-4285-ad16-ad2f3ecafbd6"),
                    Name = "Test Event 1",
                    Description = "This is a test event",
                    ImageUrl = "https://img.jpg",
                    IsDeleted = false
                }
            };

            IQueryable<Event> eventsQueryable = events.BuildMock();
            this.eventRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(eventsQueryable);

            DetailsEventViewModel? detailsEventViewModel = await this.eventService.GetEventDetailsByIdAsync("44ced817-d057-4285-ad16-ad2f3ecafbd6");

            Assert.IsNotNull(detailsEventViewModel, "Expected non-null ViewModel when event is found");
            Assert.AreEqual(detailsEventViewModel.Name, "Test Event 1");
            Assert.AreEqual(detailsEventViewModel.Description, "This is a test event");
            Assert.AreEqual(detailsEventViewModel.ImageUrl, "https://img.jpg");
        }

        [Test]
        public async Task GetEventDetailsByIdAsync_ShouldReplaceNullImage_WhenImageUrlIsNull()
        {
            List<Event> events = new List<Event>()
            {
                new Event()
                {
                    Id = Guid.Parse("44ced817-d057-4285-ad16-ad2f3ecafbd6"),
                    Name = "Test Event 1",
                    Description = "This is a test event",
                    ImageUrl = null,
                    IsDeleted = false
                }
            };

            IQueryable<Event> eventsQueryable = events.BuildMock();
            this.eventRepositoryMock
                .Setup(er => er.GetAllAttached())
                .Returns(eventsQueryable);

            DetailsEventViewModel? detailsEventViewModel = await this.eventService.GetEventDetailsByIdAsync("44ced817-d057-4285-ad16-ad2f3ecafbd6");

            Assert.IsNotNull(detailsEventViewModel, "Expected non-null ViewModel when event is found");
            Assert.AreEqual(detailsEventViewModel.ImageUrl, "/images/no-image.jpg");
        }
    }
}
