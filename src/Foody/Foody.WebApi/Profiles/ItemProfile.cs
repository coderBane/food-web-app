using AutoMapper;
using Foody.Data.Interfaces;


namespace Foody.WebApi.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            this.AddGlobalIgnore("ImageUpload");

            CreateMap<Category, CategoryDto>();

            CreateMap<Category, CategoryDetailDto>();
            CreateMap<ProductImage, Image>();

            CreateMap<CategoryModDto, Category>(MemberList.Source);

            CreateMap<Product, ProductDto>()
                .ForMember(dto => dto.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Product, ProductDetailDto>()
                .ForMember(m => m.Category, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<int, Category>().ConvertUsing<CategoryCoverter>();
            CreateMap<ProductModDto, Product>(MemberList.Source);
        }
    }

    public class CategoryCoverter : ITypeConverter<int, Category>
    {
        private readonly IUnitofWork _unitofWork;

        public CategoryCoverter(IUnitofWork unitofWork) => _unitofWork = unitofWork;

        public Category Convert(int source, Category destination, ResolutionContext context)
        {
            return _unitofWork.Categories.Exists(source)
                ? _unitofWork.Categories.Find(x => x.Id == source).Single()
                : throw new NullReferenceException("Category is null.", new ArgumentException($"CategoryId '{source}' is not valid."));
        }
    }
}
