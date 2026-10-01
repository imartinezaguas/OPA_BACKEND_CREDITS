namespace Opa.Credits.Application.DTOs;

public class CreateCreditDto
{
    public string AssociateIdentification { get; set; }
    public string AssociateName { get; set; }
    public string CreditType { get; set; }
    public decimal RequestedValue { get; set; }
    public decimal InterestRate { get; set; }
    public int NumberOfInstallments { get; set; }
    public string PaymentMethod { get; set; }
}
