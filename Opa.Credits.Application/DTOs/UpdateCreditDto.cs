namespace Opa.Credits.Application.DTOs;

public class UpdateCreditDto
{
    public decimal RequestedValue { get; set; }
    public decimal InterestRate { get; set; }
    public int NumberOfInstallments { get; set; }
    public string PaymentMethod { get; set; }
}
