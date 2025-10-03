using Application.DTO;
using Application.Interfaces;
using InventoryApp.ViewModel;

namespace InventoryApp.Provider
{
    public class CategoryProvider
    {
        private readonly ICategoryService _categoryService;

        public CategoryProvider(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<List<CategoryViewModel>> GetAllAsync(string? search = null)
        {
            var dtos = await _categoryService.GetAllAsync(search);
            return dtos.Select (dto => new CategoryViewModel 
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive
            }).ToList();
        }
        public async Task<CategoryViewModel?> GetByIdAsync(int id) 
        {
            var dto = await _categoryService.GetByIdAsync(id);
            if (dto == null) return null;
            return new CategoryViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive
            };
        }
        public async Task <int> CreateAsync(CategoryViewModel model) 
        {
            var dto = new CategoryDto
            {
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive
            };
           return await _categoryService.AddAsync(dto);
        }
        public async Task UpdateAsync(CategoryViewModel model) 
        {
            var dto = new CategoryDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive
            };
            await _categoryService.UpdateAsync(dto);
        }
        public async Task DeleteAsync(int id) 
        {
            await _categoryService.DeleteAsync(id);
        }
    }
}
