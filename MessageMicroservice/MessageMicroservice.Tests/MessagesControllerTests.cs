using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using MessageMicroservice.API.Controllers;
using MessageMicroservice.Domain.Models;
using MessageMicroservice.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace MessageMicroservice.Tests
{
    public class MessagesControllerTests
    {
        [Theory]
        [InlineData(10,10)]
        [InlineData(5,5)]
        public async Task GetAll_Returns_Correct_Number_of_Messages(int count, int size)
        {
            var fakeService = A.Fake<IMessageService>();
            var messages = A.CollectionOfDummy<Message>(count).ToList();
            A.CallTo(() => fakeService.GetAllAsync()).Returns(Task.FromResult<List<Message>>(messages));
            var controller = new MessagesController(fakeService);
            var result = await controller.GetAll();
            var okResult = result.Result as OkObjectResult;
            var listMessages = okResult.Value as List<Message>;
            Assert.Equal(size, listMessages.Count());
        }

        [Fact]
        public async Task GetById_Return_Correct_Message()
        {
            var fakeService = A.Fake<IMessageService>();
            var message = A.Dummy<Message>();
            var id = Guid.NewGuid().ToString();
            message.Id = id;
            A.CallTo(() => fakeService.GetByIdAsync(id)).Returns(Task.FromResult<Message>(message));
            var controller = new MessagesController(fakeService);
            var result = await controller.GetById(id);
            var okResult = result.Result as OkObjectResult;
            var messageResult = okResult.Value as Message;
            Assert.Equal(id, messageResult.Id);
            Assert.Equal(200, okResult.StatusCode);
        }
    }
}