using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using ProductDAOEntity = NorthwindSystem.Data.Entities.Product;
using CategoryDAOEntity = NorthwindSystem.Data.Entities.Category;
using SupplierDAOEntity = NorthwindSystem.Data.Entities.Supplier;
using NorthwindSystem.Data.DTOModels;

namespace NorthwindSystem.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Product mapping configuaration
            CreateMap<ProductDAOEntity, ProductDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(d => d.SupplierName, opt => opt.MapFrom(src => src.Supplier.CompanyName));
            //.ReverseMap();

            CreateMap<ProductDto, ProductDAOEntity>()
                .ForMember(d => d.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.ProductName, opt => opt.MapFrom(src => src.Name));
                //.ReverseMap();

            #endregion

            CreateMap<CategoryDAOEntity, CategoryDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.CategoryName))
                .ReverseMap();
            CreateMap<SupplierDAOEntity, SupplierDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.SupplierId))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.CompanyName))
                .ReverseMap();
        }
    }
}
