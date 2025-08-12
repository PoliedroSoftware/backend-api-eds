using Poliedro.Eds.Domain.Common.Events;

namespace Poliedro.Eds.Domain.Business.Events;

public class BusinessCreated : DomainEvent
{
    public string Name { get; }
    public string Context { get; }
    public int BusinessId { get; }

    public BusinessCreated(string name, string context, int businessId)
    {
        Name = name;
        Context = context;
        BusinessId = businessId;
    }
}
