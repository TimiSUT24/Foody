using Application.Ingredient.Dto.Request;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Ingredient.Mapper
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            //Request
            CreateMap<CreateIngredientDto, Domain.Models.Ingredient>();

            //Response
        }
    }
}
