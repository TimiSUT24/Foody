using Application.Category.Dto.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Category.Mapper
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            //request 

            //response
            CreateMap<Domain.Models.SubCategory, SubCategoryResponse>();

            CreateMap<Domain.Models.SubSubCategory, SubSubCategoryResponse>();
            CreateMap<Domain.Models.Category, CategoryResponse>();

            CreateMap<Domain.Models.Category, CategoryTreeResponse>()
                .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(s => s.SubCategories));

            CreateMap<Domain.Models.SubCategory, SubCategoryTreeResponse>()
                .ForMember(dest => dest.SubSubCategories, opt => opt.MapFrom(s => s.SubSubCategories));
        }
    }
}
