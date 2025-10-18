using Application.DTO;
using Application.Interfaces;
using InventoryApp.Helper;
using InventoryApp.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryApp.Provider
{
    public class ProductProvider
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductProvider(IProductService productService , ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public async Task<List<ProductViewModel>> GetAllAsync(string? search=null) 
        {
            var dtos = await _productService.GetAllAsync(search);
            return dtos.Select(p => new ProductViewModel 
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                SalePrice = p.SalePrice,
                Unit = p.Unit,
                Quantity = p.Quantity,
                ImageUrl = p.ImageUrl,
                Description = p.Description,
                CategoryId = p.CategoryId,
                CategoryName = p.CategoryName,
                IsActive = p.IsActive,
            }).ToList();
        }
        public async Task<ProductViewModel?> GetByIdAsync(int id) 
        {
            var dto = await _productService.GetByIdAsync(id);
            if (dto == null) return null;
            var categories = await _categoryService.GetAllAsync();
            var viewModel = new ProductViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Code = dto.Code,
                SalePrice = dto.SalePrice,
                Unit = dto.Unit,
                Quantity = dto.Quantity,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                CategoryName = dto.CategoryName,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
            };
            await FillDropdownsAsync(viewModel);
            return viewModel;
        }
        public async Task<ProductViewModel> GetCreateAsync() 
        {
            var model = new ProductViewModel();
            await FillDropdownsAsync(model);
            return model;
        }
        public async Task <int> CreateAsync(ProductViewModel model)
        {
            if (model.ImageFile !=null) 
            { 
                string fileName = FilelHelper.UploadFile(model.ImageFile,"images");
                model.ImageUrl = "/Files/images/" + fileName;
            }
            var dto = new ProductDto
            {
                Name = model.Name,
                Code = model.Code,
                SalePrice = model.SalePrice,
                Unit = model.Unit,
                ImageUrl = model.ImageUrl,
                Quantity =model.Quantity,
                Description = model.Description,
                CategoryId = model.CategoryId,

                IsActive = model.IsActive
            };
          return  await _productService.AddAsync(dto);
        }
        public async Task UpdateAsync(ProductViewModel model) 
        {
            if (model.ImageFile !=null) 
            {
                if (!string.IsNullOrEmpty(model.ImageUrl)) 
                {
                    string oldFileName = model.ImageUrl.TrimStart('/').Replace("images/","") ;
                    FilelHelper.DeleteFile(oldFileName, "images");
                }
                string fileName = FilelHelper.UploadFile(model.ImageFile, "images");
                model.ImageUrl = "/Files/images/" + fileName;
            }
            var dto = new ProductDto
            {
                Id = model.Id,
                Name = model.Name,
                Code = model.Code,
                SalePrice = model.SalePrice,
                Unit = model.Unit,
                ImageUrl = model.ImageUrl,
                Quantity = model.Quantity,
                Description = model.Description,
                CategoryId = model.CategoryId,
                IsActive = model.IsActive

            };
            await _productService.UpdateAsync(dto);
        }
        public async Task DeleteAsync(int id)
        {
            var vm = await GetByIdAsync(id);
            if (!string.IsNullOrEmpty(vm.ImageUrl)) 
            {
                string fileName =  vm.ImageUrl.TrimStart('/').Replace("images/","");
              FilelHelper.DeleteFile(fileName, "images");    
            }
            await _productService.DeleteAsync(id);
        }
        
        private async Task FillDropdownsAsync(ProductViewModel model) 
        {
            var categories = await _categoryService.GetAllAsync();
            model.CategoryList=categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
            }).ToList();
        }
    }
}
