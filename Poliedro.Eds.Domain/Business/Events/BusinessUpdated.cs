using Poliedro.Eds.Domain.Common.Events;

namespace Poliedro.Eds.Domain.Business.Events;

public class BusinessUpdated : DomainEvent
{
    public int BusinessId { get; }
    public string OldName { get; }
    public string OldContext { get; }
    public string NewName { get; }
    public string NewContext { get; }

    public BusinessUpdated(int businessId, string oldName, string oldContext, string newName, string newContext)
    {
        BusinessId = businessId;
        OldName = oldName;
        OldContext = oldContext;
        NewName = newName;
        NewContext = newContext;
    }
}
