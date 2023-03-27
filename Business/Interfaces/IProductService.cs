using Business.Models;
using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProductService : ICrud<ProductModel>
    {
        Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch);
        Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync();
        Task<ProductCategoryModel> GetProductCategoriesByIdAsync(int categoryId);
        Task AddCategoryAsync(ProductCategoryModel categoryModel);
        Task UpdateCategoryAsync(int id,ProductCategoryModel categoryModel);
        Task RemoveCategoryAsync(int categoryId);
        ProductModel mappToProductModel(CreateProductModel model);
        ProductCategoryModel mappToProductCategoryModel(CreateProductCategoryModel model);
        Product mappToProduct(CreateProductModel model);
        ProductCategory mappToProductCategory(CreateProductCategoryModel model);
    }
}
