using Application.Order.Dto.Request;
using Application.Order.Dto.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Mapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            //request
            CreateMap<CreateOrderDto, Domain.Models.OrderItem>();


            //response
            CreateMap<Domain.Models.Order, OrderResponse>()
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(o => o.OrderStatus.ToString()));

            CreateMap<Domain.Models.OrderItem, OrderItemsResponse>()
            .ForMember(dest => dest.FoodName, opt => opt.MapFrom(src => src.Food.Name));


            CreateMap<Domain.Models.Order, UserOrderResponse>()
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
             .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(o => o.OrderStatus.ToString()))
             .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(o => o.OrderItems));


            CreateMap<Domain.Models.OrderItem, UserOrderItemsResponse>()
            .ForMember(dest => dest.FoodName, opt => opt.MapFrom(src => src.Food.Name))
            .ForSourceMember(src => src.Order, opt => opt.DoNotValidate());
        }
    }
}
