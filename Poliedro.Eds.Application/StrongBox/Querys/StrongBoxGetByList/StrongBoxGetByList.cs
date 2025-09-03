using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Poliedro.Eds.Application.StrongBox.Dtos;

namespace Poliedro.Eds.Application.StrongBox.Querys.StrongBoxGetList;

public record StrongBoxGetList(
    int Page = 1,
    int PageSize = 20,
    long? IdCorte = null,
    string? Type = null,
    DateTime? From = null,
    DateTime? To = null
    ) : IRequest<List<StrongBoxDto>>;

