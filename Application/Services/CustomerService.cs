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
    public class CustomerService :ICustomerService
    {
        private readonly IGenericRepository<Customer> _customerRepo;

        public CustomerService(IGenericRepository<Customer> customerRepo) 
        {
            _customerRepo = customerRepo;
        }

      

        public async Task DeleteAsync(int id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null) return;
                _customerRepo.Delete(customer); 
            await _customerRepo.SaveAsync();
        }

       
    
       

        public async Task<IReadOnlyList<CustomerDto>> GetAllAsync(string? search = null)
        {
            var customers = _customerRepo.Query().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                customers = customers.Where(c => c.Name.Contains(search) || (c.PhoneNumber??"").Contains(search));
            }
            return await customers.OrderBy(c=> c.Name)
                .Select(customers => new CustomerDto
                {
                    Id = customers.Id,
                    Name = customers.Name,
                    Email = customers.Email,
                    Phone = customers.PhoneNumber,
                    Address = customers.Address,
                    IsActive = customers.IsActive,
                    CreatedAt = customers.CreatedAt,
                    UpdatedAt = customers.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null) return null;
            return new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.PhoneNumber,
                Address = customer.Address,
                IsActive = customer.IsActive,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = customer.UpdatedAt
            };
        }

      
        


        public async Task UpdateAsync(CustomerDto customerDto)
        {
            var customer = await _customerRepo.GetByIdAsync(customerDto.Id); // Convert string to int
            if (customer == null) return;
            customer.Name = customerDto.Name;
            customer.Email = customerDto.Email;
            customer.Address = customerDto.Address;
            customer.PhoneNumber = customerDto.Phone;
            customer.IsActive = customerDto.IsActive;
            customer.UpdatedAt = DateTime.UtcNow;
            _customerRepo.Update(customer);
            await _customerRepo.SaveAsync();
        }

       public async Task<int> AddAsync(CustomerDto customerDto)
        {
             var Customer = new Customer
            {
                Name = customerDto.Name,
                Email = customerDto.Email,
                PhoneNumber = customerDto.Phone,
                Address = customerDto.Address,
                IsActive = customerDto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
             };
            await _customerRepo.AddAsync(Customer);
            await _customerRepo.SaveAsync();
            return Customer.Id;
        }
            
    }
}
