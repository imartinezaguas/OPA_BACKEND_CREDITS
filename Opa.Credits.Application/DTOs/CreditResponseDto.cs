namespace Opa.Credits.Application.DTOs;

public class CreditResponseDto
{
    public int Id { get; set; }
    public string CreditNumber { get; set; }
    public string AssociateIdentification { get; set; }
    public string Status { get; set; }
    public decimal RequestedValue { get; set; }
}
