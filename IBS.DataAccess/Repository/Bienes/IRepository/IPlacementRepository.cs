using IBS.DataAccess.Repository.IRepository;
using IBS.Models.Bienes;
using IBS.Models.Bienes.ViewModels;

namespace IBS.DataAccess.Repository.Bienes.IRepository
{
    public interface IPlacementRepository : IRepository<Placement>
    {
        Task<string> GenerateControlNumberAsync(int companyId, CancellationToken cancellationToken = default);

        Task UpdateAsync(PlacementViewModel viewModel, CancellationToken cancellationToken = default);

        Task RollOverAsync(Placement model, string user, CancellationToken cancellationToken = default);

        Task<string> SwappingAsync(Placement model, int companyId, string user, CancellationToken cancellationToken = default);

    }
}

