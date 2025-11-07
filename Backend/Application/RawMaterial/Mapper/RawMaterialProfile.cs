using Application.RawMaterial.Dto.Request;
using Application.RawMaterial.Dto.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RawMaterial.Mapper
{
    public class RawMaterialProfile : Profile
    {
        public RawMaterialProfile()
        {
            //Request
            CreateMap<CreateRawMaterialDto, Domain.Models.RawMaterial>();
            CreateMap<UpdateRawMaterialDto, Domain.Models.RawMaterial>();
            //Response
            CreateMap<Domain.Models.RawMaterial, RawMaterialResponse>();
        }
    }
}
