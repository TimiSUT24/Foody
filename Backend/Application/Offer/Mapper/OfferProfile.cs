using Application.Offer.Dto.Request;
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
        }
    }
}
