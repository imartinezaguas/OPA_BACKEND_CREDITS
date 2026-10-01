using System;
using System.Linq;
using System.Threading.Tasks;
using Opa.Credits.Application.DTOs;
using Opa.Credits.Application.Interfaces;
using Opa.Credits.Domain.Entities;
using Opa.Credits.Domain.Enums;
using Opa.Credits.Domain.Exceptions;

namespace Opa.Credits.Application.Services;

public class CreditService : ICreditService
{
    private readonly ICreditRepository _creditRepository;
    private readonly IAssociateRepository _associateRepository;
    private readonly IWebhookQueue _webhookQueue;

    public CreditService(ICreditRepository creditoRepository, IAssociateRepository asociadoRepository, IWebhookQueue webhookQueue)
    {
        _creditRepository = creditoRepository;
        _associateRepository = asociadoRepository;
        _webhookQueue = webhookQueue;
    }

    public async Task<CreditResponseDto> CreateCreditAsync(CreateCreditDto dto)
    {
        // Generamos el número de crédito automáticamente en el backend (Ej: CR-2026-A1B2C3)
        string numeroGenerado = $"CR-{DateTime.UtcNow.Year}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";

        var associate = await _associateRepository.GetByIdentificationAsync(dto.AssociateIdentification);
        if (associate == null)
        {
            associate = new Associate(dto.AssociateIdentification, dto.AssociateName);
            await _associateRepository.AddAsync(associate);
            await _creditRepository.SaveChangesAsync(); 
        }

        var credit = new Credit(
            numeroGenerado, associate.Id, dto.CreditType, dto.RequestedValue, 
            dto.InterestRate, dto.NumberOfInstallments, dto.PaymentMethod
        );

        await _creditRepository.AddAsync(credit);
        await _creditRepository.SaveChangesAsync();

        // 🚀 AVISAR AL MUNDO EN SEGUNDO PLANO
        await _webhookQueue.EnqueueAsync(new WebhookMessage
        {
            Event = "credito.creado",
            Data = new 
            {
                id = credit.Id,
                creditNumber = credit.CreditNumber,
                identificacionAsociado = associate.Identification,
                requestedValue = credit.RequestedValue,
                status = credit.Status.ToString().ToUpper()
            }
        });

        return new CreditResponseDto
        {
            Id = credit.Id, CreditNumber = credit.CreditNumber,
            AssociateIdentification = associate.Identification, Status = credit.Status.ToString(),
            RequestedValue = credit.RequestedValue
        };
    }

    public async Task<PaginationResponseDto<CreditResponseDto>> GetPaginatedAsync(CreditFilterDto filter)
    {
        CreditStatus? statusEnum = null;
        if (!string.IsNullOrEmpty(filter.Status) && Enum.TryParse<CreditStatus>(filter.Status, true, out var parsed))
            statusEnum = parsed;

        var result = await _creditRepository.GetPaginatedAsync(filter.Page, filter.PageSize, statusEnum, filter.Identification);

        return new PaginationResponseDto<CreditResponseDto>
        {
            TotalItems = result.Total,
            CurrentPage = filter.Page,
            TotalPages = (int)Math.Ceiling(result.Total / (double)filter.PageSize),
            Items = result.Items.Select(c => new CreditResponseDto
            {
                Id = c.Id, CreditNumber = c.CreditNumber,
                AssociateIdentification = c.Associate?.Identification,
                Status = c.Status.ToString(), RequestedValue = c.RequestedValue
            })
        };
    }

    public async Task<CreditDetailDto> GetByIdAsync(int id)
    {
        var c = await _creditRepository.GetByIdAsync(id);
        if (c == null) throw new NotFoundException("Crédito no encontrado.");

        return new CreditDetailDto
        {
            Id = c.Id, CreditNumber = c.CreditNumber,
            AssociateIdentification = c.Associate?.Identification, AssociateName = c.Associate?.Name,
            CreditType = c.CreditType, RequestedValue = c.RequestedValue,
            InterestRate = c.InterestRate, NumberOfInstallments = c.NumberOfInstallments,
            PaymentMethod = c.PaymentMethod, Status = c.Status.ToString(),
            RequestDate = c.RequestDate, UpdateDate = c.UpdateDate,
            Histories = c.Histories.Select(h => new CreditHistoryDto
            {
                PreviousStatus = h.PreviousStatus.ToString(), NewStatus = h.NewStatus.ToString(),
                ChangeDate = h.ChangeDate, Observation = h.Observation, User = h.User
            })
        };
    }

    public async Task UpdateAsync(int id, UpdateCreditDto dto)
    {
        var credit = await _creditRepository.GetByIdAsync(id);
        if (credit == null) throw new NotFoundException("Crédito no encontrado.");

        credit.UpdateInformation(dto.RequestedValue, dto.InterestRate, dto.NumberOfInstallments, dto.PaymentMethod);
        await _creditRepository.UpdateAsync(credit);
        await _creditRepository.SaveChangesAsync();
    }

    public async Task ChangeStatusAsync(int id, ChangeStatusDto dto)
    {
        var credit = await _creditRepository.GetByIdAsync(id);
        if (credit == null) throw new NotFoundException("Crédito no encontrado.");

        if (!Enum.TryParse<CreditStatus>(dto.NewStatus, true, out var statusEnum))
            throw new BusinessRuleException("Estado inválido.");

        credit.ChangeStatus(statusEnum, dto.Observation);
        await _creditRepository.UpdateAsync(credit);
        await _creditRepository.SaveChangesAsync();

        // 🚀 AVISAR AL MUNDO EN SEGUNDO PLANO
        await _webhookQueue.EnqueueAsync(new WebhookMessage
        {
            Event = "credito.estado_cambiado",
            Data = new 
            {
                id = credit.Id,
                creditNumber = credit.CreditNumber,
                identificacionAsociado = credit.Associate?.Identification,
                requestedValue = credit.RequestedValue,
                status = statusEnum.ToString().ToUpper()
            }
        });
    }

    public async Task DeleteAsync(int id)
    {
        var credit = await _creditRepository.GetByIdAsync(id);
        if (credit == null) throw new NotFoundException("Crédito no encontrado.");

        credit.ChangeStatus(CreditStatus.Cancelled, "Solicitud de crédito cancelada.");
        await _creditRepository.UpdateAsync(credit);
        await _creditRepository.SaveChangesAsync();
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        var count = await _creditRepository.GetCountByStatusAsync();
        var summary = new DashboardSummaryDto();
        
        foreach (var item in count)
        {
            summary.QuantityByStatus[item.Key.ToString()] = item.Value;
            summary.TotalCredits += item.Value;
        }
        
        return summary;
    }
}
