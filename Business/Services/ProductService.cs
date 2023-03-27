using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public ProductService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            this._unitOfWork = _unitOfWork;
            this._mapper = _mapper;
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var obj = await _unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ProductModel>(obj);
        }
        
        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var obj = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<ProductModel>>(obj);
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var obj = await _unitOfWork.ProductCategoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductCategoryModel>>(obj);
        }
        public async Task<ProductCategoryModel> GetProductCategoriesByIdAsync(int categoryId)
        {
            var obj = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(categoryId);
            return _mapper.Map<ProductCategoryModel>(obj);
        }
        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var allProducts = await _unitOfWork.ProductRepository.GetAllWithDetailsAsync();
            var ProductList = new List<Product>();
            if (filterSearch.MinPrice == null && filterSearch.MaxPrice == null && filterSearch.CategoryId != null)
            {
                ProductList = allProducts.Where(x => x.ProductCategoryId == filterSearch.CategoryId).ToList();
            }
            else if (filterSearch.MinPrice == null && filterSearch.CategoryId == null && filterSearch.MaxPrice != null)
            {
                ProductList = allProducts.Where(x => x.Price <= filterSearch.MaxPrice).ToList();
            }
            else if (filterSearch.CategoryId == null && filterSearch.MaxPrice == null && filterSearch.MinPrice != null)
            {
                ProductList = allProducts.Where(x => x.Price >= filterSearch.MinPrice).ToList();
            }
            else if(filterSearch.CategoryId == null && filterSearch.MinPrice != null && filterSearch.MaxPrice != null)
            {
                ProductList = allProducts.Where(x => x.Price >= filterSearch.MinPrice && x.Price <= filterSearch.MaxPrice).ToList();
            }
            else if (filterSearch.CategoryId != null && filterSearch.MinPrice == null && filterSearch.MaxPrice != null)
            {
                ProductList = allProducts.Where(x => x.ProductCategoryId == filterSearch.CategoryId && x.Price <= filterSearch.MaxPrice).ToList();
            }
            else if (filterSearch.CategoryId != null && filterSearch.MinPrice != null && filterSearch.MaxPrice == null)
            {
                ProductList = allProducts.Where(x => x.ProductCategoryId == filterSearch.CategoryId && x.Price >= filterSearch.MinPrice).ToList();
            }
            else if(filterSearch.CategoryId != null && filterSearch.MinPrice != null && filterSearch.MaxPrice != null)
            {
                ProductList = allProducts.Where(x => x.Price >= filterSearch.MinPrice && x.ProductCategoryId == filterSearch.CategoryId && x.Price <= filterSearch.MaxPrice).ToList();
            }
            else
            {
                ProductList = allProducts.ToList();
            }
            return _mapper.Map<List<ProductModel>>(ProductList);
        }
        public async Task RemoveCategoryAsync(int categoryId)
        {
            var productCategory = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(categoryId);
            _unitOfWork.ProductCategoryRepository.Delete(productCategory);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(int id,ProductModel model)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdWithDetailsAsync(id);
            var category = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(model.ProductCategoryId);
            product.Price = model.Price;
            product.ProductName= model.ProductName;
            product.ProductCategoryId = model.ProductCategoryId;
            product.Category = category;
            _unitOfWork.ProductRepository.Update(product);
        }

        public async Task UpdateCategoryAsync(int id,ProductCategoryModel categoryModel)
        {
            var category = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(id);
            category.CategoryName = categoryModel.CategoryName;

            _unitOfWork.ProductCategoryRepository.Update(category);
        }

        public async Task AddAsync(ProductModel model)
        {
            _unitOfWork.ProductRepository.Add(_mapper.Map<Product>(model));
            await _unitOfWork.SaveAsync();
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            _unitOfWork.ProductCategoryRepository.Add(_mapper.Map<ProductCategory>(categoryModel));
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteAsync(int modelId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdWithDetailsAsync(modelId);
            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.SaveAsync();
        }

        public ProductModel mappToProductModel(CreateProductModel model)
        {
            return _mapper.Map<ProductModel>(model);
        }

        public ProductCategoryModel mappToProductCategoryModel(CreateProductCategoryModel model)
        {
            return _mapper.Map<ProductCategoryModel>(model);
        }

        public Product mappToProduct(CreateProductModel model)
        {
            var productModel = mappToProductModel(model);
            return _mapper.Map<Product>(productModel);
        }

        public ProductCategory mappToProductCategory(CreateProductCategoryModel model)
        {
            var productModel = mappToProductCategoryModel(model);
            return _mapper.Map<ProductCategory>(productModel);
        }
    }
}
