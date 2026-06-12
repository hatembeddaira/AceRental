using AceRental.Api.Controllers.v1;
using AceRental.Application.Clients.Command;
using AceRental.Application.Clients.Dtos;
using AceRental.Application.Clients.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AceRental.Tests
{
    public class ClientsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ClientsController _controller;

        public ClientsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ClientsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Get_ReturnsAllClients()
        {
            // Arrange
            var clients = new List<ClientDto> { new ClientDto(){ Id = new Guid(), FirstName = "John", LastName = "Doe", Email = "", ClientNumber = 0 }, new ClientDto() { Id = new Guid(), FirstName = "John", LastName = "Doe", Email = "", ClientNumber = 0 }}.AsQueryable();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllClientsQuery>(), default(CancellationToken)))
                         .ReturnsAsync(clients);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedClients = Assert.IsAssignableFrom<IQueryable<ClientDto>>(okResult.Value);
            Assert.Equal(2, returnedClients.Count());
        }

        [Fact]
        public async Task Get_WithExistingId_ReturnsClient()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var client = new ClientDto { Id = clientId, FirstName = "John", LastName = "Doe", Email = "", ClientNumber = 0 };
            var clients = new List<ClientDto> { client }.AsQueryable();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllClientsQuery>(), default(CancellationToken)))
                         .ReturnsAsync(clients);

            // Act
            var result = await _controller.Get(clientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedClient = Assert.IsType<ClientDto>(okResult.Value);
            Assert.Equal(clientId, returnedClient.Id);
        }

        [Fact]
        public async Task Get_WithNonExistingId_ReturnsNoContent()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            var clients = new List<ClientDto>().AsQueryable();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllClientsQuery>(), default(CancellationToken)))
                         .ReturnsAsync(clients);

            // Act
            var result = await _controller.Get(nonExistingId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Post_WithValidCommand_ReturnsCreated()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var command = new CreateClientCommand(clientId.ToString(), "John", "Doe", "", "", "", "", "", "", "");
            var clientDto = new ClientDto { Id = clientId, FirstName = "John", LastName = "Doe", Email = "", ClientNumber = 0 };
            _mediatorMock.Setup(m => m.Send(command, default(CancellationToken)))
                        .ReturnsAsync(clientDto);

            // Act
            var result = await _controller.Post(command);

            // Assert
            var createdResult = Assert.IsType<CreatedODataResult<ClientDto>>(result);
            var returnedClient = Assert.IsType<ClientDto>(createdResult.Entity);
            
            Assert.Equal(clientDto.Id, returnedClient.Id);
        }
    }
}
