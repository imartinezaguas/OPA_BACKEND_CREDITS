namespace Opa.Credits.Application.DTOs;

public class CreditFilterDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Status { get; set; }
    public string? Identification { get; set; }
}
