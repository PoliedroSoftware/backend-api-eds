using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.TypeOfCollection.DomainTypeOfCollection;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;

namespace Poliedro.Eds.Application.TypeOfCollection.Commands.CreateTypeOfCollection;
    public class CreateTypeOfCollectionCommandHandler(
        ITypeOfCollectionCreateTypeOfCollection TypeOfCollectionDomainTypeOfCollection,
        IMapper mapper) : IRequestHandler<CreateTypeOfCollectionCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(CreateTypeOfCollectionCommand request, CancellationToken cancellationToken)
        {
            var TypeOfCollectionEntity = mapper.Map<TypeOfCollectionEntity>(request.Request);
            var result = await TypeOfCollectionDomainTypeOfCollection.CreateAsync(TypeOfCollectionEntity);
            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }