using System;
using System.Collections.Generic;
using Opa.Credits.Domain.Enums;
using Opa.Credits.Domain.Exceptions;

namespace Opa.Credits.Domain.Entities;

public class Credit
{
    public int Id { get; private set; }
    public string CreditNumber { get; private set; }
    
    public int AssociateId { get; private set; }
    public Associate Associate { get; private set; }
    
    public string CreditType { get; private set; }
    public decimal RequestedValue { get; private set; }
    public decimal InterestRate { get; private set; }
    public int NumberOfInstallments { get; private set; }
    public string PaymentMethod { get; private set; }
    public CreditStatus Status { get; private set; }
    
    public DateTime RequestDate { get; private set; }
    public DateTime UpdateDate { get; private set; }
    


    public virtual ICollection<CreditHistory> Histories { get; private set; } = new List<CreditHistory>();

    protected Credit() { }

    public Credit(string creditNumber, int associateId, string creditType, decimal requestedValue, decimal interestRate, int numberOfInstallments, string paymentMethod)
    {
        // Validaciones del Dominio requeridas por la prueba técnica
        if (requestedValue <= 0) throw new BusinessRuleException("El valor solicitado debe ser mayor a 0.");
        if (interestRate < 0) throw new BusinessRuleException("La tasa de interés no puede ser negativa.");
        if (numberOfInstallments <= 0) throw new BusinessRuleException("El número de cuotas debe ser mayor a 0.");

        CreditNumber = creditNumber ?? throw new ArgumentNullException(nameof(creditNumber));
        AssociateId = associateId;
        CreditType = creditType;
        RequestedValue = requestedValue;
        InterestRate = interestRate;
        NumberOfInstallments = numberOfInstallments;
        PaymentMethod = paymentMethod;
        
        Status = CreditStatus.Requested;
        RequestDate = DateTime.UtcNow;
        UpdateDate = DateTime.UtcNow;
    }

    public void ChangeStatus(CreditStatus newStatus, string observation, string modificationUser = "Sistema")
    {
        // Regla de Negocio: Transiciones inválidas
        if (Status == CreditStatus.Disbursed && (newStatus == CreditStatus.Rejected || newStatus == CreditStatus.Cancelled))
            throw new BusinessRuleException("Un crédito Desembolsado no puede ser Rechazado ni Cancelado.");

        if (Status == CreditStatus.Rejected && newStatus == CreditStatus.Disbursed)
            throw new BusinessRuleException("No se puede desembolsar un crédito que ya fue rechazado.");
            
        if (Status == CreditStatus.Cancelled)
            throw new BusinessRuleException("No se puede cambiar el estado de un crédito cancelado.");
            
        var previousStatus = Status;
        Status = newStatus;
        UpdateDate = DateTime.UtcNow;

        // Dejamos registro automático en la misma transacción lógica
        Histories.Add(new CreditHistory(this, previousStatus, newStatus, observation, modificationUser));
    }

    public void UpdateInformation(decimal requestedValue, decimal interestRate, int numberOfInstallments, string paymentMethod)
    {
        if (Status == CreditStatus.Disbursed || Status == CreditStatus.Cancelled || Status == CreditStatus.Rejected)
            throw new BusinessRuleException("No se puede modificar la información de un crédito que ya fue Desembolsado, Rechazado o Cancelado.");

        if (requestedValue <= 0) throw new BusinessRuleException("El valor solicitado debe ser mayor a 0.");
        if (interestRate < 0) throw new BusinessRuleException("La tasa de interés no puede ser negativa.");
        if (numberOfInstallments <= 0) throw new BusinessRuleException("El número de cuotas debe ser mayor a 0.");

        RequestedValue = requestedValue;
        InterestRate = interestRate;
        NumberOfInstallments = numberOfInstallments;
        PaymentMethod = paymentMethod;
        UpdateDate = DateTime.UtcNow;
    }
}
