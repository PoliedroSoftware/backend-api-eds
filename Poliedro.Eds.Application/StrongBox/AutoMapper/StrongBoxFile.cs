using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Poliedro.Eds.Application.StrongBox.Dtos;
using Poliedro.Eds.Domain.StrongBox.Entities;

namespace Poliedro.Eds.Application.StrongBox.AutoMapper
{
    public class StrongBoxFile : Profile
    {
        public StrongBoxFile()
        {
            CreateMap<StrongBoxEntity, StrongBoxDto>();
        }
    }
}
