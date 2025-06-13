using AutoMapper;
using MediatR;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.Common.Results.Errors;
using Poliedro.Eds.Domain.Court.DomainService;
using Poliedro.Eds.Domain.Court.Entities;

namespace Poliedro.Eds.Application.Court.Commands.UpdateCourt
{
    public class UpdateCourtCommandHandler : IRequestHandler<UpdateCourtCommand, Result<VoidResult, Error>>
    {
        private readonly ICourtUpdateService courtUpdateService;
        private readonly IMapper mapper;

        public UpdateCourtCommandHandler(ICourtUpdateService courtUpdateService, IMapper mapper)
        {

            this.courtUpdateService = courtUpdateService;
            this.mapper = mapper;
        }

        public async Task<Result<VoidResult, Error>> Handle(UpdateCourtCommand request, CancellationToken cancellationToken)
        {
            var courtEntity = mapper.Map<CourtEntity>(request);


            var result = await courtUpdateService.UpdateAsync(courtEntity);


            if (!result.IsSuccess)
                return result.Error!;


            return result.Value!;
        }
    }
}
