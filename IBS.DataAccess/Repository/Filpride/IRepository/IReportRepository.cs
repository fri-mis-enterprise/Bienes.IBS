using IBS.DataAccess.Repository.IRepository;
using IBS.Models.Filpride.AccountsPayable;
using IBS.Models.Filpride.AccountsReceivable;
using IBS.Models.Filpride.Books;
using IBS.Models.Filpride.Integrated;
using IBS.Models.Filpride.ViewModels;

namespace IBS.DataAccess.Repository.Filpride.IRepository
{
    public interface IReportRepository : IRepository<GeneralLedgerBook>
    {
        Task<List<ReceivingReport>> GetReceivingReportAsync(DateOnly? dateFrom, DateOnly? dateTo, string? selectedFiltering, string company, CancellationToken cancellationToken = default);

        List<Inventory> GetInventoryBooks(DateOnly dateFrom, DateOnly dateTo, string company);

        Task<List<GeneralLedgerBook>> GetGeneralLedgerBooks(DateOnly dateFrom, DateOnly dateTo, string company, CancellationToken cancellationToken = default);

        Task<List<AuditTrail>> GetAuditTrails(DateOnly dateFrom, DateOnly dateTo, string company);

        Task<List<CustomerOrderSlip>> GetCosUnservedVolume(DateOnly dateFrom, DateOnly dateTo, string company);

        public Task<List<SalesReportViewModel>> GetSalesReport(DateOnly dateFrom, DateOnly dateTo, string company, List<int>? commissioneeIds = null, string statusFilter = "ValidOnly", CancellationToken cancellationToken = default);

        public Task<List<SalesInvoice>> GetSalesInvoiceReport(DateOnly dateFrom, DateOnly dateTo, string company, string statusFilter = "ValidOnly", CancellationToken cancellationToken = default);

        public Task<List<PurchaseOrder>> GetPurchaseOrderReport(DateOnly dateFrom, DateOnly dateTo, string company, string statusFilter = "ValidOnly", CancellationToken cancellationToken = default);

        public Task<List<CheckVoucherHeader>> GetClearedDisbursementReport(DateOnly dateFrom, DateOnly dateTo, string company, CancellationToken cancellationToken = default);

        public Task<List<ReceivingReport>> GetPurchaseReport(DateOnly dateFrom, DateOnly dateTo, string company, List<int>? customerIds = null, List<int>? commissioneeIds = null, string dateSelectionType = "RRDate", string statusFilter = "ValidOnly", CancellationToken cancellationToken = default);

        public Task<List<DeliveryReceipt>> GetGrossMarginReport( DateOnly dateFrom, DateOnly dateTo, string company, List<int>? customers = null, List<int>? commissionee = null, CancellationToken cancellationToken = default);
        public Task<List<CollectionReceipt>> GetCollectionReceiptReport(DateOnly dateFrom, DateOnly dateTo, string company, string statusFilter = "ValidOnly", CancellationToken cancellationToken = default);

        public Task<List<ReceivingReport>> GetTradePayableReport(DateOnly dateFrom, DateOnly dateTo, string company, CancellationToken cancellationToken = default);

        public Task<List<DeliveryReceipt>> GetHaulerPayableReport(DateOnly dateFrom, DateOnly dateTo, string company, CancellationToken cancellationToken = default);

        public Task<List<ServiceInvoice>> GetServiceInvoiceReport(DateOnly dateFrom, DateOnly dateTo, string company, string statusFilter = "ValidOnly", CancellationToken cancellationToken = default);

        public Task<List<PurchaseOrder>> GetApReport(DateOnly monthYear, string company, CancellationToken cancellationToken = default);

        public Task<List<SalesInvoice>> GetARPerCustomerReport(DateOnly dateFrom, DateOnly dateTo, string company, List<int>? customerIds = null, string statusFilter = "ValidOnly", CancellationToken cancellationToken = default);

        public Task<List<JournalVoucherDetail>> GetJournalVoucherReport(DateOnly dateFrom, DateOnly dateTo, string company, string statusFilter = "ValidOnly", CancellationToken cancellationToken = default);

        public Task<List<CustomerOrderSlip>> GetCustomerOrderSlipReport(DateOnly dateFrom, DateOnly dateTo, string company, string statusFilter = "ValidOnly", CancellationToken cancellationToken = default);
    }
}
