using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poliedro.Eds.Domain.Common.Results;
using Poliedro.Eds.Domain.RegisterShift.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Poliedro.Eds.Domain.RegisterShift.DomainService;

public interface IRegisterShiftCreateService
{
    Task<Result<VoidResult, Error>> CreateAsync(RegisterShiftEntity registerShiftEntity);
}
