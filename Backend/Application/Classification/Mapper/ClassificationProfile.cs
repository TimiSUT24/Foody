using Application.Classification.Dto.Request;
using Application.Classification.Dto.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Classification.Mapper
{
    public class ClassificationProfile : Profile
    {
        public ClassificationProfile()
        {
            //Request
            CreateMap<CreateClassificationDto, Domain.Models.Classification>();
            CreateMap<UpdateClassificationDto, Domain.Models.Classification>();

            //Response
            CreateMap<Domain.Models.Classification, ClassificationResponse>();
        }
    }
}
