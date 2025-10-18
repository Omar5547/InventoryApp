using Application.DTO;
using Application.Interfaces;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
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
                City = customer.City,

                Country = customer.Country,
                Fax = customer.Fax,
                Discount = customer.Discount,
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
            customer.City = customerDto.City;
            customer.Country = customerDto.Country;
            customer.Fax = customerDto.Fax;
            customer.Discount = customerDto.Discount;
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
                City = customerDto.City,
                Country = customerDto.Country,
                Fax = customerDto.Fax,
                Discount = customerDto.Discount,
                 IsActive = customerDto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
             };
            await _customerRepo.AddAsync(Customer);
            await _customerRepo.SaveAsync();
            return Customer.Id;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllAsync(string? search)
        {
            var customers = _customerRepo.Query().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                customers = customers.Where(v => v.Name.Contains(search) || (v.PhoneNumber ?? "").Contains(search));
            }
            return await customers.OrderBy(c => c.Name)
                .Select(customers => new CustomerDto
                {
                    Id = customers.Id,
                    Name = customers.Name,
                    Email = customers.Email,
                    Phone = customers.PhoneNumber,
                    Address = customers.Address,
                    City = customers.City,
                    Country = customers.Country,
                    Fax = customers.Fax,
                    Discount = customers.Discount,
                    IsActive = customers.IsActive,
                    CreatedAt = customers.CreatedAt,
                    UpdatedAt = customers.UpdatedAt
                })
                .ToListAsync();
        }
    }
}
