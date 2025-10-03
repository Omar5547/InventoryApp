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
    public class LocationService : ILocationService
    {
        private readonly IGenericRepository<Location> _locationRepo;

        public LocationService(IGenericRepository<Location> locationRepo) 
        {
            _locationRepo = locationRepo;
        }
        public async Task<int> AddAsync(LocationDto locationDto)
        {
            var location = new Location
            {
                Name = locationDto.Name, 
                Address = locationDto.Address,
                IsActive = locationDto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _locationRepo.AddAsync(location);
            await _locationRepo.SaveAsync();
            return location.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var location = await _locationRepo.GetByIdAsync(id);
            if (location == null) return;
            _locationRepo.Delete(location);
            await _locationRepo.SaveAsync();
        }

        public async Task<IReadOnlyList<LocationDto>> GetAllAsync(string? search = null)
        {
            var location = _locationRepo.Query().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                location = location.Where(l => l.Name.Contains(search));
            }
            return await location.OrderBy(l=> l.Name)
                .Select(location => new LocationDto
                {
                    Id = location.Id,
                    Name = location.Name,
                    Address = location.Address,
                    IsActive = location.IsActive,
                    CreatedAt = location.CreatedAt,
                    UpdatedAt = location.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<LocationDto?> GetByIdAsync(int id)
        {
            var location = await _locationRepo.GetByIdAsync(id);
            if (location == null) return null;
            return new LocationDto
            {
                Id = location.Id,
                Name = location.Name,
                Address = location.Address,
                IsActive = location.IsActive,
                CreatedAt = location.CreatedAt,
                UpdatedAt = location.UpdatedAt
            };
        }

        public async Task UpdateAsync(LocationDto locationDto)
        {
            var location = await _locationRepo.GetByIdAsync(locationDto.Id);
            if (location == null) return;
            location.Name = locationDto.Name;
            location.Address = locationDto.Address;
            location.IsActive = locationDto.IsActive;
            location.UpdatedAt = DateTime.UtcNow;
            _locationRepo.Update(location);
            await _locationRepo.SaveAsync();
        }
    }
}
