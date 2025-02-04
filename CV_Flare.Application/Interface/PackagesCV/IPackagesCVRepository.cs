using CV_FLare.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV_Flare.Application.Interface.PackagesCV
{
    public interface IPackagesCVRepository
    {
        Task<IEnumerable<Package>> GetAllPackagesCV();
        Task<Package> GetPackagesCVById(int id);
        Task<Package> AddPackagesCV(Package package);
        Task<Package> UpdatePackagesCV(int id, Package package);
        Task<Package> DeletePackagesCV(int id);
    }
}
