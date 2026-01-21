using Application.Offer.Dto.Request;
using Application.Offer.Dto.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Offer.Mapper
{
    public class OfferProfile : Profile
    {
        public OfferProfile()
        {
            //request 
            CreateMap<AddOfferDto, Domain.Models.Offer>();


            //response
            CreateMap<Domain.Models.Offer, OfferResponseDto>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => DateTime.UtcNow >= src.StartsAtUtc 
                                                                         && DateTime.UtcNow <= src.EndsAtUtc));
           
        }
    }
}
