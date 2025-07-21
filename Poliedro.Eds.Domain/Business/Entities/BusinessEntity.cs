using Poliedro.Eds.Domain.Business.Exepction;
using System.ComponentModel.DataAnnotations;

namespace Poliedro.Eds.Domain.Business.Entities;

public class BusinessEntity
{
        [Key]
        public int IdBusiness { get; private set; }
        public string Name { get; private set; }
        public string Context { get; private set; }

        private BusinessEntity(string name, string context)
        {
            Validate(name, context);

            Name = name;
            Context = context;
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

        
        protected BusinessEntity() { }
    }