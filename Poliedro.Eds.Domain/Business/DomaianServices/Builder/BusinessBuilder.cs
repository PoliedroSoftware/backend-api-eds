using Poliedro.Eds.Domain.Business.Entities;

namespace Poliedro.Eds.Domain.Business.DomaianServices.Builder;

public class BusinessBuilder
{
    private string _name = string.Empty;
    private string _context = string.Empty;

    public BusinessBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public BusinessBuilder WithContext(string context)
    {
        _context = context;
        return this;
    }

    public BusinessEntity Build()
    {
        return BusinessEntity.Create(_name, _context);
    }
}