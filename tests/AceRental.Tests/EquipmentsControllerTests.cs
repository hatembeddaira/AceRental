using AceRental.Api.Controllers.v1;
using AceRental.Application.Equipments.Command;
using AceRental.Application.Equipments.Dtos;
using AceRental.Application.Equipments.Queries;
using AceRental.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AceRental.Tests
{
    public class EquipmentsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly EquipmentsController _controller;

        public EquipmentsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new EquipmentsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Get_ReturnsAllEquipments()
        {
            // Arrange
            var equipments = new List<EquipmentDetailsDto> { new EquipmentDetailsDto { Reference = "Ref1", Name = "Name1" }, new EquipmentDetailsDto { Reference = "Ref2", Name = "Name2" }}.AsQueryable();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllEquipmentsQuery>(), default(CancellationToken)))
                         .ReturnsAsync(equipments);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEquipments = Assert.IsAssignableFrom<IQueryable<EquipmentDetailsDto>>(okResult.Value);
            Assert.Equal(2, returnedEquipments.Count());
        }

        [Fact]
        public async Task Get_WithExistingId_ReturnsEquipment()
        {
            // Arrange
            var equipmentId = Guid.NewGuid();
            var equipments = new List<EquipmentDetailsDto> { new EquipmentDetailsDto { Id = equipmentId, Reference = "Ref1", Name = "Name1" } }.AsQueryable();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllEquipmentsQuery>(), default(CancellationToken)))
                         .ReturnsAsync(equipments);

            // Act
            var result = await _controller.Get(equipmentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEquipment = Assert.IsType<EquipmentDetailsDto>(okResult.Value);
            Assert.Equal(equipmentId, returnedEquipment.Id);
        }

        [Fact]
        public async Task Get_WithNonExistingId_ReturnsNoContent()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            var equipments = new List<EquipmentDetailsDto>().AsQueryable();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllEquipmentsQuery>(), default(CancellationToken)))
                         .ReturnsAsync(equipments);

            // Act
            var result = await _controller.Get(nonExistingId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Availability_ReturnsAvailabilityCount()
        {
            // Arrange
            var equipmentId = Guid.NewGuid();
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(1);
            var availabilityQuery = new GetEquipmentAvailabilityQuery(equipmentId, startDate, endDate);
            var expectedAvailability = 5;

            _mediatorMock.Setup(m => m.Send(It.Is<GetEquipmentAvailabilityQuery>(q => q.EquipmentId == equipmentId && q.StartDate == startDate && q.EndDate == endDate), default(CancellationToken)))
                         .ReturnsAsync(expectedAvailability);

            // Act
            var result = await _controller.Availability(equipmentId, startDate, endDate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAvailability = Assert.IsType<int>(okResult.Value);
            Assert.Equal(expectedAvailability, returnedAvailability);
        }

        [Fact]
        public async Task Post_WithValidCommand_ReturnsCreated()
        {
            // Arrange
            var command = new CreateEquipmentCommand("Ref1", "Name1", "", 100.0m, 100.0m, 100.0m, 10, EquipmentCategory.Son);
            var equipmentDto = new EquipmentDetailsDto { Id = Guid.NewGuid(), Reference = "Ref1", Name = "Name1" };
            _mediatorMock.Setup(m => m.Send(command, default(CancellationToken)))
                        .ReturnsAsync(equipmentDto);

            // Act
            var result = await _controller.Post(command);

            // Assert
            var createdResult = Assert.IsType<CreatedODataResult<EquipmentDetailsDto>>(result);
            var returnedEquipment = Assert.IsType<EquipmentDetailsDto>(createdResult.Entity);
            Assert.Equal(equipmentDto.Id, returnedEquipment.Id);
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
