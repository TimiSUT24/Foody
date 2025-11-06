using Application.Auth.Dto.Request;
using Application.Auth.Dto.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Mapper
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            //Request 
            CreateMap<RegisterDto, Domain.Models.User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<LoginDto, Domain.Models.User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            //Response
            CreateMap<Domain.Models.User, RegisterResponseDto>();
            CreateMap<Domain.Models.User, LoginDtoResponse>();
        }
    }
}
