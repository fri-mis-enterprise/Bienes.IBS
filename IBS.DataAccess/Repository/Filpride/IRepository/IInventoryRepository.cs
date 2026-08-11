using IBS.DataAccess.Repository.IRepository;
using IBS.Models.Filpride.AccountsPayable;
using IBS.Models.Filpride.Books;
using IBS.Models.Filpride.Integrated;

namespace IBS.DataAccess.Repository.Filpride.IRepository
{
    public interface IInventoryRepository : IRepository<Inventory>
    {
        Task AddPurchaseToInventoryAsync(ReceivingReport receivingReport, CancellationToken cancellationToken = default);

        Task AddSalesToInventoryAsync(DeliveryReceipt deliveryReceipt, CancellationToken cancellationToken = default);

        Task VoidInventory(Inventory model, CancellationToken cancellationToken = default);

        Task ReCalculateInventoryAsync(List<Inventory> inventories, CancellationToken cancellationToken = default);
    }
}
