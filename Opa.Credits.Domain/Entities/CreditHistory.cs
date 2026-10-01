using System;
using Opa.Credits.Domain.Enums;

namespace Opa.Credits.Domain.Entities;

public class CreditHistory
{
    public int Id { get; private set; }
    
    public int CreditId { get; private set; }
    public Credit Credit { get; private set; }
    
    public CreditStatus PreviousStatus { get; private set; }
    public CreditStatus NewStatus { get; private set; }
    public DateTime ChangeDate { get; private set; }
    public string Observation { get; private set; }
    public string User { get; private set; }

    protected CreditHistory() { }

    public CreditHistory(Credit credit, CreditStatus previousStatus, CreditStatus newStatus, string observation, string user)
    {
        Credit = credit ?? throw new ArgumentNullException(nameof(credit));
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
        Observation = observation;
        User = user;
        ChangeDate = DateTime.UtcNow;
    }
}
