using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Data;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ForMember(rm=>rm.CustomerId,r=>r.MapFrom(x=>x.CustomerId))
                .ReverseMap();
            CreateMap<ReceiptDetailModel, CreateReceiptDetailModel>()
            .ReverseMap();

            CreateMap<ReceiptModel, CreateReceiptModel>()
                .ReverseMap();

            CreateMap<CustomerModel, CreateCustomerModel>()
             .ReverseMap();

            CreateMap<ProductModel, CreateProductModel>()
            .ReverseMap();

            CreateMap<ProductCategoryModel, CreateProductCategoryModel>()
            .ReverseMap();

            CreateMap<Product, ProductModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<ProductModel, Product>();

            CreateMap<ReceiptDetail, ReceiptDetailModel>()
                .ReverseMap();

            CreateMap<ProductCategory, ProductCategoryModel>()
               .ForMember(rm => rm.ProductIds, r => r.MapFrom(x => x.Products.Select(y => y.Id)))
               .ReverseMap();

            CreateMap<ProductCategoryModel, ProductCategory>();

            CreateMap<Customer, CustomerModel>()
            .ForMember(rm => rm.Name, r => r.MapFrom(x => x.Person.Name))
            .ForMember(rm => rm.Surname, r => r.MapFrom(x => x.Person.Surname))
            .ForMember(rm => rm.ReceiptsIds, r => r.MapFrom(x => x.Receipts.Select(y => y.Id)))
            .ForMember(rm => rm.BirthDate, r => r.MapFrom(x => x.Person.BirthDate))
            .ReverseMap();
        }
    }
}