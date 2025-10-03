using Application.DTO;
using Application.Interfaces;
using InventoryApp.ViewModel;

namespace InventoryApp.Provider
{
    public class CustomerProvider
    {
        private readonly ICustomerService _customerService;

        public CustomerProvider(ICustomerService customerService) 
        {
            _customerService = customerService;
        }
        public async Task<List<CustomerViewModel>> GetAllAsync(string? search = null)
        {
            var dtos = await _customerService.GetAllAsync();
            return dtos.Select(dto => new CustomerViewModel 
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                Address = dto.Address,
                Phone = dto.Phone,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt

            }).ToList();
        }
        public async Task<CustomerViewModel?> GetByIdAsync(int id) 
        {
            var dto = await _customerService.GetByIdAsync(id);
            if (dto == null) return null;
            return new CustomerViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Email = dto.Email,
                Address = dto.Address,
                Phone = dto.Phone,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt

            };
        }
        public async Task <int> CreateAsync(CustomerViewModel model) 
        {
            var dto = new CustomerDto
            {
                Name = model.Name,
                Email = model.Email,
                Address = model.Address,
                Phone = model.Phone,
                IsActive = model.IsActive
            };
           return await _customerService.AddAsync(dto);

        }
        public async Task UpdateAsync(CustomerViewModel model) 
        {
            var dto = new CustomerDto
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                IsActive = model.IsActive



            };
            await _customerService.UpdateAsync(dto);
        }
        public async Task DeleteAsync(int id) 
        {
            await _customerService.DeleteAsync(id);
        }
    }
}
