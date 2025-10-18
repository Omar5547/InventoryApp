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
    public class VendorService : IVendorService
    {
        private readonly IGenericRepository<Vendor> _vendorRepo;

        public VendorService(IGenericRepository<Vendor> vendorRepo) 
        {
            _vendorRepo = vendorRepo;
        }
        public async Task<int> AddAsync(VendorDto vendorDto)
        {
            var vendor = new Vendor
            {
                Name = vendorDto.Name,
                Email = vendorDto.Email,
                Phone = vendorDto.Phone,
                Address = vendorDto.Address,
                City = vendorDto.City,
                Country = vendorDto.Country,
                Fax = vendorDto.Fax,
                Discount = vendorDto.Discount,
                
                IsActive = vendorDto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _vendorRepo.AddAsync(vendor);
            await _vendorRepo.SaveAsync();
            return vendor.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var vendor = await _vendorRepo.GetByIdAsync(id);
            if (vendor == null) return;
            _vendorRepo.Delete(vendor);
            await _vendorRepo.SaveAsync();
        }

        

        public async Task<VendorDto?> GetByIdAsync(int id)
        {
            var vendor = await _vendorRepo.GetByIdAsync(id);
            if (vendor == null) return null;
            return new VendorDto
            {
                Id = vendor.Id,
                Name = vendor.Name,
                Email = vendor.Email,
                Phone = vendor.Phone,

                Address = vendor.Address,
                City = vendor.City,
                Country = vendor.Country,
                Fax = vendor.Fax,
                Discount = vendor.Discount,

                IsActive = vendor.IsActive,
                CreatedAt = vendor.CreatedAt,
                UpdatedAt = vendor.UpdatedAt
            };
        }

        public async Task UpdateAsync(VendorDto vendorDto)
        {
            var vendor = await _vendorRepo.GetByIdAsync(vendorDto.Id); 
            if (vendor == null) return;
            vendor.Name = vendorDto.Name;
            vendor.Email = vendorDto.Email;
            vendor.Address = vendorDto.Address;
            vendor.Phone= vendorDto.Phone;
            vendor.City = vendorDto.City;
            vendor.Country = vendorDto.Country;
            vendor.Fax = vendorDto.Fax;
            vendor.Discount = vendorDto.Discount;

            vendor.IsActive = vendorDto.IsActive;
            vendor.UpdatedAt = DateTime.UtcNow;
            _vendorRepo.Update(vendor);
            await _vendorRepo.SaveAsync();
        }

        public async Task<IEnumerable<VendorDto>>GetAllAsync(string? search)
        {
            var vendor = _vendorRepo.Query().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                vendor = vendor.Where(v => v.Name.Contains(search) || (v.Phone ?? "").Contains(search));
            }
            return await vendor.OrderBy(c => c.Name)
                .Select(vendor => new VendorDto
                {
                    Id = vendor.Id,
                    Name = vendor.Name,
                    Email = vendor.Email,
                    Phone = vendor.Phone,
                    Address = vendor.Address,
                    City = vendor.City,
                    Country = vendor.Country,
                    Fax = vendor.Fax,
                    Discount = vendor.Discount,

                    IsActive = vendor.IsActive,
                    CreatedAt = vendor.CreatedAt,
                    UpdatedAt = vendor.UpdatedAt
                })
                .ToListAsync();
        }
    }
}
