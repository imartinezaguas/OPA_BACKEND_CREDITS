using System.Collections.Generic;

namespace Opa.Credits.Application.DTOs;

public class DashboardSummaryDto
{
    public int TotalCredits { get; set; }
    public Dictionary<string, int> QuantityByStatus { get; set; } = new Dictionary<string, int>();
}
