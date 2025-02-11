using CV_Flare.Application.Interface.PackagesCV;
using CV_FLare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Application.Services.PackagesCV
{
    public class PackagesCVService : IPackagesCVService
    {
        private readonly IPackagesCVRepository _packagesCVRepository;


        public PackagesCVService(IPackagesCVRepository packagesCVRepository)
        {
            _packagesCVRepository = packagesCVRepository;
        }

        public Task<Package> AddPackagesCV(Package package)
        {
            return _packagesCVRepository.AddPackagesCV(package);
        }

        public Task<Package> DeletePackagesCV(int id)
        {
            return _packagesCVRepository.DeletePackagesCV(id);
        }

        public Task<IEnumerable<Package>> GetAllPackagesCV()
        {
            return _packagesCVRepository.GetAllPackagesCV();
        }

        public Task<Package> GetPackagesCVById(int id)
        {
            return _packagesCVRepository.GetPackagesCVById(id);
        }

        public Task<Package> UpdatePackagesCV(int id, Package package)
        {
            return _packagesCVRepository.UpdatePackagesCV(id, package);
        }
    }
}
