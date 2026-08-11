using IBS.DataAccess.Repository.Bienes.IRepository;
using IBS.DataAccess.Repository.Filpride.IRepository;
using IBS.DataAccess.Repository.MasterFile.IRepository;
using IBS.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IBS.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        MasterFile.IRepository.IProductRepository Product { get; }

        ICompanyRepository Company { get; }

        Task SaveAsync(CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetProductListAsyncByCode(CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetProductListAsyncById(CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetChartOfAccountListAsyncByNo(CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetChartOfAccountListAsyncById(CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetChartOfAccountListAsyncByAccountTitle(CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetCompanyListAsyncByName(CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetCompanyListAsyncById(CancellationToken cancellationToken = default);

        #region--Filpride

        Filpride.IRepository.IChartOfAccountRepository ChartOfAccount { get; }
        Filpride.IRepository.ICustomerOrderSlipRepository CustomerOrderSlip { get; }
        IDeliveryReceiptRepository DeliveryReceipt { get; }
        Filpride.IRepository.ISupplierRepository Supplier { get; }
        Filpride.IRepository.ICustomerRepository Customer { get; }
        IAuditTrailRepository AuditTrail { get; }
        ICustomerBranchRepository CustomerBranch { get; }
        ITermsRepository Terms { get; }
        Filpride.IRepository.IGeneralLedgerRepository GeneralLedger { get; }
        IProvisionalReceiptRepository ProvisionalReceipt { get; }
        ILockedPeriodAdjustmentRepository LockedPeriodAdjustment { get; }
        IDepartmentAccessRepository DepartmentAccess { get; }

        Task<List<SelectListItem>> GetFilprideCustomerListAsyncById(string company, CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetFilprideSupplierListAsyncById(string company, CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetFilprideEmployeeSupplierListAsyncById(string company, CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetFilprideTradeSupplierListAsyncById(string company, CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetFilprideNonTradeSupplierListAsyncById(string company, CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetFilprideCommissioneeListAsyncById(string company, CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetFilprideHaulerListAsyncById(string company, CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetFilprideBankAccountListById(string company, CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetDistinctFilpridePickupPointListById(string company, CancellationToken cancellationToken = default);

        Task<List<SelectListItem>> GetFilprideServiceListById(string company, CancellationToken cancellationToken = default);

        #endregion

        #region AAS

        #region Accounts Receivable
        ISalesInvoiceRepository SalesInvoice { get; }

        Filpride.IRepository.IServiceInvoiceRepository ServiceInvoice { get; }

        Filpride.IRepository.ICollectionReceiptRepository CollectionReceipt { get; }

        Filpride.IRepository.IDebitMemoRepository DebitMemo { get; }

        Filpride.IRepository.ICreditMemoRepository CreditMemo { get; }
        #endregion

        #region Accounts Payable

        Filpride.IRepository.ICheckVoucherRepository FilprideCheckVoucher { get; }

        Filpride.IRepository.IJournalVoucherRepository FilprideJournalVoucher { get; }

        Filpride.IRepository.IPurchaseOrderRepository PurchaseOrder { get; }

        Filpride.IRepository.IReceivingReportRepository ReceivingReport { get; }

        #endregion

        #region Books and Report
        Filpride.IRepository.IInventoryRepository Inventory { get; }

        IReportRepository FilprideReport { get; }
        #endregion

        #region Master File

        Filpride.IRepository.IBankAccountRepository BankAccount { get; }

        Filpride.IRepository.IServiceRepository Service { get; }

        Filpride.IRepository.IPickUpPointRepository PickUpPoint { get; }

        IFreightRepository Freight { get; }

        IAuthorityToLoadRepository AuthorityToLoad { get; }

        #endregion

        #endregion

        #region --Bienes

        IPlacementRepository Placement { get; }

        #endregion

        INotificationRepository Notifications { get; }

        Task<bool> IsPeriodPostedAsync(DateOnly date, CancellationToken cancellationToken = default);

        Task<DateTime> GetMinimumPeriodBasedOnThePostedPeriods(Module module, CancellationToken cancellationToken = default);

        Task<bool> IsPeriodPostedAsync(Module module, DateOnly date, CancellationToken cancellationToken = default);
    }
}

