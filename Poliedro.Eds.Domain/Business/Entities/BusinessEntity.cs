using System.ComponentModel.DataAnnotations;
using Poliedro.Eds.Domain.Common.Entities;
using Poliedro.Eds.Domain.Business.Events;
using Poliedro.Eds.Domain.Business.Exepction;

namespace Poliedro.Eds.Domain.Business.Entities;

public class BusinessEntity : AggregateRoot
{
    [Key]
    public int IdBusiness { get; private set; }
    public string Name { get; private set; } = null!;
    public string Context { get; private set; } = null!;

    private BusinessEntity(string name, string context)
    {
        Validate(name, context);

        Name = name;
        Context = context;
        CreatedAt = DateTime.UtcNow;
        AddDomainEvent(new BusinessCreated(name, context, IdBusiness));
    }

    public void Update(string name, string? context)
    {
        var oldName = Name;
        var oldContext = Context;
        
        Validate(name, context ?? string.Empty);
        
        Name = name;
        Context = context ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
        
        if (oldName != name || oldContext != Context)
        {
            AddDomainEvent(new BusinessUpdated(IdBusiness, oldName, oldContext, name, Context));
        }
    }

    public static BusinessEntity Create(string name, string context)
    {
        return new BusinessEntity(name, context);
    }

    private static void Validate(string name, string context)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new BusinessDomainException("El nombre del negocio no puede estar vacío.");

        if (name.Length > 100)
            throw new BusinessDomainException("El nombre del negocio no puede tener más de 100 caracteres.");

        if (string.IsNullOrWhiteSpace(context))
            throw new BusinessDomainException("El contexto del negocio no puede estar vacío.");
    }

    // Constructor para EF Core
    protected BusinessEntity() { }
}


