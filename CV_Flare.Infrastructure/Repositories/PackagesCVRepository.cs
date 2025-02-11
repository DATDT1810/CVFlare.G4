using CV_Flare.Application.Interface.PackagesCV;
using CV_Flare.Infrastructure.DB;
using CV_FLare.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Infrastructure.Repositories
{
    public class PackagesCVRepository : IPackagesCVRepository
    {
        private readonly ApplicationDbContext _context;

        public PackagesCVRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Package> AddPackagesCV(Package package)
        {
            await _context.AddAsync(package);
            await _context.SaveChangesAsync();
            return package;
        }

        public async Task<Package> DeletePackagesCV(int id)
        {
            var packagesCV = await _context.Packages.FindAsync(id);
            if (packagesCV == null)
            {
                throw new KeyNotFoundException($"Package with ID {id} not found.");
            }
            _context.Packages.Remove(packagesCV);
            await _context.SaveChangesAsync();
            return packagesCV;
        }

        public async Task<IEnumerable<Package>> GetAllPackagesCV()
        {
            return await _context.Packages.ToListAsync();
        }

        public async Task<Package> GetPackagesCVById(int id)
        {
            var packagesCV = await _context.Packages.FindAsync(id);
            if (packagesCV == null)
            {
                throw new KeyNotFoundException($"Package with ID {id} not found.");
            }
            return packagesCV;
        }

        public async Task<Package> UpdatePackagesCV(int id, Package package)
        {
            var existingPackagesCV = await _context.Packages.FindAsync(id);
            if (existingPackagesCV == null)
            {
                throw new KeyNotFoundException($"Package with ID {id} not found.");
            }
            existingPackagesCV.PackageName = package.PackageName;
            existingPackagesCV.PackageDescription = package.PackageDescription;
            existingPackagesCV.PackagePrice = package.PackagePrice;
            existingPackagesCV.UpdateAt = DateTime.UtcNow;
            _context.Packages.Update(package);
            await _context.SaveChangesAsync();
            return existingPackagesCV;
        }
    }
}
