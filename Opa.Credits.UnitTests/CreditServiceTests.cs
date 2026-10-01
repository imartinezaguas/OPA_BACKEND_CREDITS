using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Xunit;
using Opa.Credits.Application.Services;
using Opa.Credits.Application.Interfaces;
using Opa.Credits.Application.DTOs;
using Opa.Credits.Domain.Entities;
using Opa.Credits.Domain.Enums;
using Opa.Credits.Domain.Exceptions;

namespace Opa.Credits.UnitTests;

public class CreditoServiceTests
{
    private readonly Mock<ICreditRepository> _creditoRepoMock;
    private readonly Mock<IAssociateRepository> _asociadoRepoMock;
    private readonly Mock<IWebhookQueue> _webhookQueueMock;
    private readonly CreditService _service;

    public CreditoServiceTests()
    {
        _creditoRepoMock = new Mock<ICreditRepository>();
        _asociadoRepoMock = new Mock<IAssociateRepository>();
        _webhookQueueMock = new Mock<IWebhookQueue>();

        _service = new CreditService(
            _creditoRepoMock.Object,
            _asociadoRepoMock.Object,
            _webhookQueueMock.Object
        );
    }

    [Fact]
    public async Task CrearCredito_CreditoNuevo_DebeCrearYEncolarWebhook()
    {
        // Arrange
        var dto = new CreateCreditDto
        {
            AssociateIdentification = "123456",
            AssociateName = "Juan Perez",
            CreditType = "Consumo",
            RequestedValue = 5000,
            InterestRate = 1.5m,
            NumberOfInstallments = 12,
            PaymentMethod = "Mensual"
        };

        _asociadoRepoMock.Setup(r => r.GetByIdentificationAsync(dto.AssociateIdentification)).ReturnsAsync((Associate?)null);

        // Act
        var result = await _service.CreateCreditAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.CreditNumber.Should().StartWith("CR-");
        
        _creditoRepoMock.Verify(r => r.AddAsync(It.IsAny<Credit>()), Times.Once);
        // Se llama dos veces a SaveChanges: una por el Associate nuevo y otra por el Crédito
        _creditoRepoMock.Verify(r => r.SaveChangesAsync(), Times.Exactly(2)); 
        _webhookQueueMock.Verify(w => w.EnqueueAsync(It.IsAny<WebhookMessage>()), Times.Once);
    }


    [Fact]
    public async Task ObtenerPorId_IdInexistente_DebeLanzarNotFoundException()
    {
        // Arrange
        _creditoRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Credit?)null);

        // Act
        Func<Task> action = async () => await _service.GetByIdAsync(99);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Crédito no encontrado.");
    }

    [Fact]
    public async Task CambiarEstado_TransicionValida_DebeCambiarEstadoYEncolarWebhook()
    {
        // Arrange
        var credit = new Credit("CRED-100", 1, "Vivienda", 1000, 1.0m, 10, "Mensual"); // Nace en Requested
        _creditoRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(credit);

        var dto = new ChangeStatusDto { NewStatus = "Approved", Observation = "Todo ok" };

        // Act
        await _service.ChangeStatusAsync(1, dto);

        // Assert
        credit.Status.Should().Be(CreditStatus.Approved);
        _creditoRepoMock.Verify(r => r.UpdateAsync(credit), Times.Once);
        _creditoRepoMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        
        // El Webhook de status cambiado se debió encolar
        _webhookQueueMock.Verify(w => w.EnqueueAsync(It.Is<WebhookMessage>(m => m.Event == "credito.estado_cambiado")), Times.Once);
    }

    [Fact]
    public async Task CambiarEstado_TransicionInvalida_DebeLanzarBusinessRuleException()
    {
        // Arrange
        var credit = new Credit("CRED-100", 1, "Vivienda", 1000, 1.0m, 10, "Mensual"); 
        credit.ChangeStatus(CreditStatus.Rejected, "Rechazado"); // Lo forzamos a Rejected
        
        _creditoRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(credit);

        // Intentamos un salto ilegal de Rejected a Disbursed
        var dto = new ChangeStatusDto { NewStatus = "Disbursed", Observation = "Ilegal" };

        // Act
        Func<Task> action = async () => await _service.ChangeStatusAsync(1, dto);

        // Assert
        await action.Should().ThrowAsync<BusinessRuleException>()
            .WithMessage("*No se puede desembolsar un crédito que ya fue rechazado.*");
            
        // El webhook nunca debió salir
        _webhookQueueMock.Verify(w => w.EnqueueAsync(It.IsAny<WebhookMessage>()), Times.Never);
    }
}
