using Application.DTO;
using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepo;

        public CategoryService(IGenericRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<int> AddAsync(CategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                IsActive = categoryDto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow

            };
            await _categoryRepo.AddAsync(category);
            await _categoryRepo.SaveAsync();
            return category.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return;
            _categoryRepo.Delete(category);
            await _categoryRepo.SaveAsync();
        }


       

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return null;
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt

            };
        }

        
        public async Task UpdateAsync(CategoryDto categoryDto)
        {
            var category = await _categoryRepo.GetByIdAsync(categoryDto.Id);
            if (category == null) return;
            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            category.IsActive = categoryDto.IsActive;
            category.UpdatedAt = DateTime.UtcNow;
            _categoryRepo.Update(category);
            await _categoryRepo.SaveAsync();
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(string? search)
        {
            var categories = _categoryRepo.Query().AsNoTracking();
            if (!string.IsNullOrEmpty(search))
            {
                categories = categories.Where(c => c.Name.Contains(search));
            }
            return await categories.OrderBy(c => c.Name)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    IsActive = c.IsActive,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync();
        }
    }
}
