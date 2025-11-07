using Application.NutritionValue.Dto.Request;
using Application.NutritionValue.Dto.Response;
using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.NutritionValue.Mapper
{
    public class NutritionValueProfile : Profile
    {
        public NutritionValueProfile()
        {
            //request
            CreateMap<CreateNutritionValueDto, Domain.Models.NutritionValue>();
            CreateMap<UpdateNutritionValueDto, Domain.Models.NutritionValue>();

            //response
            CreateMap<Domain.Models.NutritionValue, NutritionValueResponse>();
        }
    }
}
