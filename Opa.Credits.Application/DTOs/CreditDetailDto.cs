using System;
using System.Collections.Generic;

namespace Opa.Credits.Application.DTOs;

public class CreditDetailDto
{
    public int Id { get; set; }
    public string CreditNumber { get; set; }
    public string AssociateIdentification { get; set; }
    public string AssociateName { get; set; }
    public string CreditType { get; set; }
    public decimal RequestedValue { get; set; }
    public decimal InterestRate { get; set; }
    public int NumberOfInstallments { get; set; }
    public string PaymentMethod { get; set; }
    public string Status { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime UpdateDate { get; set; }
    
    public IEnumerable<CreditHistoryDto> Histories { get; set; }
}

public class CreditHistoryDto
{
    public string PreviousStatus { get; set; }
    public string NewStatus { get; set; }
    public DateTime ChangeDate { get; set; }
    public string Observation { get; set; }
    public string User { get; set; }
}
