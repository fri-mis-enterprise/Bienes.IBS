using IBS.DataAccess.Repository.IRepository;
using IBS.Models.Filpride.Books;
using IBS.Models.Filpride.MasterFile;

namespace IBS.DataAccess.Repository.Filpride.IRepository
{
    public interface ICustomerBranchRepository : IRepository<CustomerBranch>
    {
        Task UpdateAsync(CustomerBranch model, CancellationToken cancellationToken);
    }
}
