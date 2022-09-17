using AutoMapper;
using Foody.Entities.DTOs;
using Foody.Entities.Models;
using Foody.Data.Interfaces;


namespace Foody.WebApi.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Category, CategoryDto>();

            CreateMap<Category, CategoryDetailDto>();
            CreateMap<AppFile, Image>()
                .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dto => dto.FileSize, opt => opt.MapFrom(src => src.Size))
                .ForMember(dto => dto.Content, opt => opt.MapFrom(src => src.Content));

            CreateMap<CategoryModDto, Category>();

            CreateMap<Product, ProductDto>()
                .ForMember(dto => dto.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Product, ProductDetailDto>()
                .ForMember(m => m.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<int, Category>().ConvertUsing<CategoryCoverter>();
            CreateMap<ProductModDto, Product>();
        }
    }

    public class CategoryCoverter : ITypeConverter<int, Category>
    {
        private readonly IUnitofWork _unitofWork;

        public CategoryCoverter(IUnitofWork unitofWork) => _unitofWork = unitofWork;

        public Category Convert(int source, Category destination, ResolutionContext context)
        {
            return _unitofWork.Categories.Exists(source)
                ? _unitofWork.Categories.Find(x => x.Id == source).First()
                : throw new NullReferenceException("Category is null.", new ArgumentException($"CategoryId '{source}' is not valid."));
        }
    }
}
