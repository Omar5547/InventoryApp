using Application.DTO;
using Application.Interfaces;
using InventoryApp.ViewModel;

namespace InventoryApp.Provider
{
    public class VendorProvider
    {
        private readonly IVendorService _vendorService;

        public VendorProvider(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }
        public async Task<List<VendorViewModel>> GetAllAsync(string? search = null ) 
        {
            var dtos = await _vendorService.GetAllAsync(search);
            return dtos.Select(dto => new VendorViewModel 
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,

            }).ToList();
        }
        public async Task<VendorViewModel?> GetByIdAsync(int id) 
        {
            var dto = await _vendorService.GetByIdAsync(id);
            if (dto == null) return null;
            return new VendorViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
            };
        }
        public async Task <int> CreateAsync(VendorViewModel Model) 
        {
            var dto = new VendorDto
            {
                Name = Model.Name,
                Email = Model.Email,
                Phone = Model.Phone,
                Address = Model.Address,
                IsActive = Model.IsActive

            };
            return await _vendorService.AddAsync(dto);
        }
        public async Task UpdateAsync(VendorViewModel model) 
        {
            var dto = new VendorDto
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                IsActive = model.IsActive
            };
            await _vendorService.UpdateAsync( dto);
        }
        public async Task DeleteAsync(int id) 
        {
            await _vendorService.DeleteAsync(id);
        }
    }
}
