using Application.Product.Dto.Request;
using Application.Product.Dto.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            //Requests
            CreateMap<CreateProductDto, Domain.Models.Product>();//Mapping request DTO to Domain Model
            CreateMap<UpdateProductDto, Domain.Models.Product>();

            //Responses
            CreateMap<Domain.Models.Product,ProductResponseDto>();

        }
    }
}
