using AceRental.Api.Controllers.v1;
using AceRental.Application.Invoices.Command;
using AceRental.Application.Invoices.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Results;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AceRental.Tests
{
    public class InvoicesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly InvoicesController _controller;

        public InvoicesControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new InvoicesController(_mediatorMock.Object);
        }

        [Fact]
        public async Task PartiallyInvoice_WithValidCommand_ReturnsCreated()
        {
            // Arrange
            var ReservationId = Guid.NewGuid();
            var PaymentId = Guid.NewGuid();
            var invoiceId = Guid.NewGuid();
            var command = new GeneratePartiallyInvoiceCommand(ReservationId, PaymentId, 100.0m);
            
            _mediatorMock.Setup(m => m.Send(It.IsAny<GeneratePartiallyInvoiceCommand>(), default(CancellationToken)))
                         .ReturnsAsync(invoiceId);

            // Act
            var result = await _controller.PartiallyInvoice(command);

            // Assert
            var createdResult = Assert.IsType<CreatedODataResult<Guid>>(result);
            // Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            // Assert.Equal(nameof(InvoicesController.PartiallyInvoice), createdResult.ActionName);

            // Assert.NotNull(createdResult.RouteValues);
            // Assert.True(createdResult.RouteValues.ContainsKey("id"));
            // // The current controller implementation passes the entire InvoiceDto as the 'id' route value.
            // var routeValueId = Assert.IsType<Guid>(createdResult.RouteValues["id"]);
            // Assert.Equal(invoiceId, routeValueId);

            var returnedInvoice = Assert.IsType<Guid>(createdResult.Value);
            Assert.Equal(invoiceId, returnedInvoice);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GeneratePartiallyInvoiceCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RentalInvoice_WithValidCommand_ReturnsCreated()
        {
            // Arrange
            var reservationId = Guid.NewGuid();
            var invoiceId = Guid.NewGuid();
            var command = new GenerateRentalInvoiceCommand(reservationId);

            _mediatorMock.Setup(m => m.Send(It.IsAny<GenerateRentalInvoiceCommand>(), default(CancellationToken)))
                         .ReturnsAsync(invoiceId);

            // Act
            var result = await _controller.RentalInvoice(command);

            // Assert
            var createdResult = Assert.IsType<CreatedODataResult<Guid>>(result);
            // Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            // Assert.Equal(nameof(InvoicesController.RentalInvoice), createdResult.ActionName);
            // Assert.NotNull(createdResult.RouteValues);
            // Assert.True(createdResult.RouteValues.ContainsKey("id"));
            // var routeValueId = Assert.IsType<Guid>(createdResult.RouteValues["id"]);
            // Assert.Equal(invoiceId, routeValueId);
            var returnedInvoice = Assert.IsType<Guid>(createdResult.Value);
            Assert.Equal(invoiceId, returnedInvoice);

            _mediatorMock.Verify(m => m.Send(It.IsAny<GenerateRentalInvoiceCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}