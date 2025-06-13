using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.TypeOfCollection.DomainTypeOfCollection;
using Poliedro.Eds.Domain.TypeOfCollection.Entities;

namespace Poliedro.Eds.Application.TypeOfCollection.Commands.UpdateTypeOfCollection;

    public class UpdateTypeOfCollectionCommandHandler(
        ITypeOfCollectionUpdateTypeOfCollection TypeOfCollectionDomainTypeOfCollection,
        IMapper mapper
    ) : IRequestHandler<UpdateTypeOfCollectionCommand, Result<VoidResult, Error>>
    {
        public async Task<Result<VoidResult, Error>> Handle(UpdateTypeOfCollectionCommand request, CancellationToken cancellationToken)
        {
            var TypeOfCollectionEntity = mapper.Map<TypeOfCollectionEntity>(request);
            var result = await TypeOfCollectionDomainTypeOfCollection.UpdateAsync(TypeOfCollectionEntity);

            if (!result.IsSuccess)
                return result.Error!;

            return result.Value!;
        }
    }