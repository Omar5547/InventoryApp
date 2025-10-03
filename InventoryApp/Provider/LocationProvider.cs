using Application.DTO;
using Application.Interfaces;
using InventoryApp.ViewModel;

namespace InventoryApp.Provider
{
    public class LocationProvider
    {
        private readonly ILocationService _locationService;

        public LocationProvider(ILocationService locationService)
        {
            _locationService = locationService;
        }
        public async Task<List<LocationViewModel>> GetAllAsync(string? search = null)
        {
            var dtos = await _locationService.GetAllAsync(search);
            return dtos.Select(dto => new LocationViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Address = dto.Address,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            }).ToList();
        }
        public async Task<LocationViewModel?> GetByIdAsync(int id)
        {
            var dto = await _locationService.GetByIdAsync(id);
            if (dto == null) return null;
            return new LocationViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Address = dto.Address,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }
        public async Task<int> CreateAsync(LocationViewModel model)
        {
            var dto = new LocationDto
            {
                Name = model.Name,
                Address = model.Address,
                IsActive = model.IsActive
            };
            return await _locationService.AddAsync(dto);
        }
        public async Task UpdateAsync(LocationViewModel model)
        {
            var dto = new LocationDto
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                IsActive = model.IsActive
            };
            await _locationService.UpdateAsync(dto);
        }
        public async Task DeleteAsync(int id)
        {
            await _locationService.DeleteAsync(id);
        }
    }
}
