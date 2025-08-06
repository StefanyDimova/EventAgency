//using EventAgency.Data.Models;
//using EventAgency.Services.Core.Admin.Interfaces;
//using EventAgency.Services.Core.Interfaces;
//using EventAgency.WebApi.Controllers;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace EventAgency.WebApi.Tests
//{
//    [TestFixture]
//    public class EventReservationRequestsApiControllerTests
//    {
//        private Mock<IEventReservationRequestService> serviceMock;
//        private Mock<IEventReservationManagementService> managementServiceMock;
//        private EventReservationRequestsApiController controller;

//        [SetUp]
//        public void SetUp()
//        {
//            this.serviceMock = new Mock<IEventReservationRequestService>();.
//            this.managementServiceMock = new Mock<IEventReservationManagementService>();
//            this.controller = new EventReservationRequestsApiController(this.serviceMock.Object);
//        }

//        [Test]
//        public async Task CreateRequest_NullRequest_ReturnsBadRequest()
//        {
//            ActionResult<EventReservationRequest> result = await this.controller.CreateRequest(null);
//            Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
//        }

//        [Test]
//        public async Task CreateRequest_ValidRequest_ReturnsCreatedAtAction()
//        {
//            EventReservationRequest input = new EventReservationRequest
//            {
//                RequestedDate = new DateTime(2025, 10, 10),
//                EventType = "Wedding"
//            };

//            EventReservationRequest created = new EventReservationRequest
//            {
//                Id = Guid.NewGuid(),
//                RequestedDate = input.RequestedDate,
//                EventType = input.EventType,
//                IsApproved = false
//            };

//            this.managementServiceMock
//                .Setup(s => s.AddRequestAsync(input.RequestedDate.Date, input.EventType, input.UserEmail))
//                .ReturnsAsync(created);

//            ActionResult<EventReservationRequest> result = await this.controller.CreateRequest(input);
//            CreatedAtActionResult createdResult = result.Result as CreatedAtActionResult;

//            Assert.That(createdResult, Is.Not.Null);
//            Assert.That(createdResult.Value, Is.EqualTo(created));
//        }

//        [Test]
//        public async Task CreateRequest_ServiceThrows_ReturnsBadRequest()
//        {
//            EventReservationRequest input = new EventReservationRequest
//            {
//                RequestedDate = DateTime.Today,
//                EventType = "Conference"
//            };

//            this.serviceMock
//                .Setup(s => s.AddRequestAsync(It.IsAny<DateTime>(), It.IsAny<string>()))
//                .ThrowsAsync(new Exception("DB error"));

//            ActionResult<EventReservationRequest> result = await this.controller.CreateRequest(input);
//            BadRequestObjectResult badRequest = result.Result as BadRequestObjectResult;

//            Assert.That(badRequest, Is.Not.Null);
//            Assert.That(badRequest.Value.ToString(), Does.Contain("DB error"));
//        }

//        [Test]
//        public async Task GetPendingRequests_ReturnsOkWithData()
//        {
//            List<EventReservationRequest> fakeList = new List<EventReservationRequest>
//            {
//                new EventReservationRequest { RequestedDate = DateTime.Today, EventType = "Wedding" }
//            };

//            this.serviceMock
//                .Setup(s => s.GetPendingRequestsAsync())
//                .ReturnsAsync(fakeList);

//            ActionResult<IEnumerable<EventReservationRequest>> result = await this.controller.GetPendingRequests();
//            OkObjectResult okResult = result.Result as OkObjectResult;

//            Assert.That(okResult, Is.Not.Null);
//            Assert.That(okResult.Value, Is.EqualTo(fakeList));
//        }

//        [Test]
//        public async Task GetRequestByDate_NoResult_ReturnsNotFound()
//        {
//            DateTime date = DateTime.Today;

//            this.serviceMock
//                .Setup(s => s.GetRequestByDateAsync(date))
//                .ReturnsAsync((EventReservationRequest)null!);

//            ActionResult<EventReservationRequest> result = await this.controller.GetRequestByDate(date);
//            Assert.That(result.Result, Is.InstanceOf<NotFoundObjectResult>());
//        }

//        [Test]
//        public async Task GetRequestByDate_Found_ReturnsOk()
//        {
//            DateTime date = DateTime.Today;

//            EventReservationRequest found = new EventReservationRequest
//            {
//                RequestedDate = date,
//                EventType = "Birthday"
//            };

//            this.serviceMock
//                .Setup(s => s.GetRequestByDateAsync(date))
//                .ReturnsAsync(found);

//            ActionResult<EventReservationRequest> result = await this.controller.GetRequestByDate(date);
//            OkObjectResult okResult = result.Result as OkObjectResult;

//            Assert.That(okResult, Is.Not.Null);
//            Assert.That(okResult.Value, Is.EqualTo(found));
//        }
//    }
////}

