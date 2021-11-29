using AutoMapper;
using HB.Case.Api.Models.Dtos;
using HB.Case.Models.Entities;

namespace HB.Case.Api.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
        }
    }
}
