using Application.DTO;
using Application.Interfaces;
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
        public async Task<List<ProductViewModel>> GetAllAsync(string? search=null,int? skip=null,int? take =null) 
        {
            var dtos = await _productService.GetAllAsync(search,skip,take);
            return dtos.Select(p => new ProductViewModel 
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU,
                SalePrice = p.SalePrice,
                Unit = p.Unit,
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
                SKU = dto.SKU,
                PurchasePrice = dto.PurchasePrice,
                SalePrice = dto.SalePrice,
                Unit = dto.Unit,
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
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/images");
                if(!Directory.Exists(folderPath)) 
                {
                    Directory.CreateDirectory(folderPath); // تأكد من وجود مجلد الصور
                }
                string filePath = Path.Combine(folderPath, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create)) 
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                model.ImageUrl = "/images/" + uniqueFileName;
            }
            var dto = new ProductDto
            {
                SKU = model.SKU,
                Name = model.Name,
                PurchasePrice = model.PurchasePrice,
                SalePrice = model.SalePrice,
                Unit = model.Unit,
                ImageUrl = model.ImageUrl,
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
                    string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", model.ImageUrl.TrimStart('/'));
                    if (File.Exists(oldPath)) 
                    {
                        File.Delete(oldPath);
                    }
                }
                string UniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/images" );
                if (!Directory.Exists(folderPath)) 
                {
                    Directory.CreateDirectory(folderPath);
                }
                string filePath = Path.Combine(folderPath,UniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create)) 
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                model.ImageUrl = "/images/" + UniqueFileName;
            }
            var dto = new ProductDto
            {
                Id = model.Id,
                Name = model.Name,
                SKU = model.SKU,
                PurchasePrice = model.PurchasePrice,
                SalePrice = model.SalePrice,
                Unit = model.Unit,
                ImageUrl = model.ImageUrl,
                CategoryId = model.CategoryId,

            };
            await _productService.UpdateAsync(dto);
        }
        public async Task DeleteAsync(int id)
        {
            var vm = await GetByIdAsync(id);
            if (!string.IsNullOrEmpty(vm.ImageUrl)) 
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", vm.ImageUrl.TrimStart('/'));
                if (File.Exists(path)) 
                {
                    File.Delete(path);
                }
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
