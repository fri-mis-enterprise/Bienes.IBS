using IBS.DataAccess.Data;
using IBS.DataAccess.Repository.Bienes;
using IBS.DataAccess.Repository.Bienes.IRepository;
using IBS.DataAccess.Repository.Filpride;
using IBS.DataAccess.Repository.Filpride.IRepository;
using IBS.DataAccess.Repository.IRepository;
using IBS.DataAccess.Repository.MasterFile;
using IBS.DataAccess.Repository.MasterFile.IRepository;
using IBS.Models.Enums;
using IBS.Models.Filpride.MasterFile;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq.Expressions;
using IProductRepository = IBS.DataAccess.Repository.MasterFile.IRepository.IProductRepository;
using ProductRepository = IBS.DataAccess.Repository.MasterFile.ProductRepository;

namespace IBS.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IDepartmentAccessRepository DepartmentAccess { get; }

        public INotificationRepository Notifications { get; private set; }

        public async Task<bool> IsPeriodPostedAsync(DateOnly date, CancellationToken cancellationToken = default)
        {
            return await _db.PostedPeriods
                .AnyAsync(m => m.IsPosted
                               && m.Month == date.Month
                               && m.Year == date.Year, cancellationToken);
        }

        public async Task<DateTime> GetMinimumPeriodBasedOnThePostedPeriods(Module module, CancellationToken cancellationToken = default)
        {
            if (!Enum.IsDefined(typeof(Module), module))
            {
                throw new InvalidEnumArgumentException(nameof(module), (int)module, typeof(Module));
            }

            var period = await _db.PostedPeriods
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .FirstOrDefaultAsync(x => x.Module == module.ToString()
                                          && x.IsPosted, cancellationToken);

            if (period == null)
            {
                return DateTime.MinValue;
            }

            return new DateOnly(period.Year, period.Month, 1)
                .AddMonths(1)
                .ToDateTime(new TimeOnly(0, 0));
        }

        public async Task<bool> IsPeriodPostedAsync(Module module, DateOnly date, CancellationToken cancellationToken = default)
        {
            if (!Enum.IsDefined(typeof(Module), module))
            {
                throw new InvalidEnumArgumentException(nameof(module), (int)module, typeof(Module));
            }

            return await _db.PostedPeriods
                .AnyAsync(m =>
                    m.Module == module.ToString() &&
                    m.IsPosted &&
                    m.Year == date.Year &&
                    m.Month == date.Month,
                    cancellationToken);
        }

        #region--Filpride

        public Filpride.IRepository.ICustomerOrderSlipRepository CustomerOrderSlip { get; private set; }
        public IDeliveryReceiptRepository DeliveryReceipt { get; private set; }
        public Filpride.IRepository.ICustomerRepository Customer { get; private set; }
        public Filpride.IRepository.ISupplierRepository Supplier { get; private set; }
        public Filpride.IRepository.IPickUpPointRepository PickUpPoint { get; private set; }
        public IFreightRepository Freight { get; private set; }
        public IAuthorityToLoadRepository AuthorityToLoad { get; private set; }
        public Filpride.IRepository.IChartOfAccountRepository ChartOfAccount { get; private set; }
        public IAuditTrailRepository AuditTrail { get; private set; }
        public ICustomerBranchRepository CustomerBranch { get; private set; }
        public ITermsRepository Terms { get; private set; }
        public Filpride.IRepository.IGeneralLedgerRepository GeneralLedger { get; private set; }
        public IProvisionalReceiptRepository ProvisionalReceipt { get; private set; }
        public ILockedPeriodAdjustmentRepository LockedPeriodAdjustment { get; private set; }

        #endregion

        #region AAS

        #region Accounts Receivable
        public ISalesInvoiceRepository SalesInvoice { get; private set; }

        public Filpride.IRepository.IServiceInvoiceRepository ServiceInvoice { get; private set; }

        public Filpride.IRepository.ICollectionReceiptRepository CollectionReceipt { get; private set; }

        public Filpride.IRepository.IDebitMemoRepository DebitMemo { get; private set; }

        public Filpride.IRepository.ICreditMemoRepository CreditMemo { get; private set; }
        #endregion

        #region Accounts Payable
        public Filpride.IRepository.ICheckVoucherRepository FilprideCheckVoucher { get; private set; }

        public Filpride.IRepository.IJournalVoucherRepository FilprideJournalVoucher { get; private set; }

        public Filpride.IRepository.IPurchaseOrderRepository PurchaseOrder { get; private set; }

        public Filpride.IRepository.IReceivingReportRepository ReceivingReport { get; private set; }
        #endregion

        #region Books and Report
        public Filpride.IRepository.IInventoryRepository Inventory { get; private set; }

        public IReportRepository FilprideReport { get; private set; }
        #endregion

        #region Master File

        public Filpride.IRepository.IBankAccountRepository BankAccount { get; private set; }

        public Filpride.IRepository.IServiceRepository Service { get; private set; }

        #endregion

        #endregion

        #region --Bienes

        public IPlacementRepository Placement { get; private set; }

        #endregion

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            Product = new ProductRepository(_db);
            Company = new CompanyRepository(_db);
            Notifications = new NotificationRepository(_db);
            DepartmentAccess = new DepartmentAccessRepository(_db);

            #region--Filpride

            CustomerOrderSlip = new Filpride.CustomerOrderSlipRepository(_db);
            DeliveryReceipt = new DeliveryReceiptRepository(_db);
            Customer = new Filpride.CustomerRepository(_db);
            Supplier = new Filpride.SupplierRepository(_db);
            PickUpPoint = new Filpride.PickUpPointRepository(_db);
            Freight = new FreightRepository(_db);
            AuthorityToLoad = new AuthorityToLoadRepository(_db);
            ChartOfAccount = new Filpride.ChartOfAccountRepository(_db);
            AuditTrail = new AuditTrailRepository(_db);
            CustomerBranch = new CustomerBranchRepository(_db);
            Terms = new TermsRepository(_db);
            GeneralLedger = new Filpride.GeneralLedgerRepository(_db);
            ProvisionalReceipt = new ProvisionalReceiptRepository(_db);
            LockedPeriodAdjustment = new LockedPeriodAdjustmentRepository(_db);

            #endregion

            #region AAS

            #region Accounts Receivable
            SalesInvoice = new SalesInvoiceRepository(_db);
            ServiceInvoice = new Filpride.ServiceInvoiceRepository(_db);
            CollectionReceipt = new Filpride.CollectionReceiptRepository(_db);
            DebitMemo = new Filpride.DebitMemoRepository(_db);
            CreditMemo = new Filpride.CreditMemoRepository(_db);
            #endregion

            #region Accounts Payable
            FilprideCheckVoucher = new Filpride.CheckVoucherRepository(_db);
            FilprideJournalVoucher = new Filpride.JournalVoucherRepository(_db);
            PurchaseOrder = new Filpride.PurchaseOrderRepository(_db);
            ReceivingReport = new Filpride.ReceivingReportRepository(_db);
            #endregion

            #region Books and Report
            Inventory = new Filpride.InventoryRepository(_db);
            FilprideReport = new ReportRepository(_db);
            #endregion

            #region Master File

            BankAccount = new Filpride.BankAccountRepository(_db);
            Service = new Filpride.ServiceRepository(_db);

            #endregion

            #endregion

            #region --Bienes

            Placement = new PlacementRepository(_db);

            #endregion
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await _db.SaveChangesAsync(cancellationToken);
        }

        public void Dispose() => _db.Dispose();

        #region--Filpride

        // Make the function generic
        private Expression<Func<T, bool>> GetCompanyFilter<T>(string companyName) where T : class
        {
            // Use reflection or property pattern matching to dynamically access properties
            var param = Expression.Parameter(typeof(T), "x");

            // Build the appropriate expression based on the company name
            Expression propertyAccess = companyName switch
            {
                nameof(Filpride) => Expression.Property(param, "IsFilpride"),
                nameof(Bienes) => Expression.Property(param, "IsBienes"),
                _ => Expression.Constant(false)
            };

            return Expression.Lambda<Func<T, bool>>(propertyAccess, param);
        }

        public async Task<List<SelectListItem>> GetFilprideCustomerListAsyncById(string company, CancellationToken cancellationToken = default)
        {
            return await _db.Customers
                .OrderBy(c => c.CustomerName)
                .Where(c => c.IsActive)
                .Where(GetCompanyFilter<Customer>(company))
                .Select(c => new SelectListItem
                {
                    Value = c.CustomerId.ToString(),
                    Text = c.CustomerName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SelectListItem>> GetFilprideSupplierListAsyncById(string company, CancellationToken cancellationToken = default)
        {
            return await _db.Suppliers
                .OrderBy(s => s.SupplierCode)
                .Where(s => s.IsActive)
                .Where(GetCompanyFilter<Supplier>(company))
                .Select(s => new SelectListItem
                {
                    Value = s.SupplierId.ToString(),
                    Text = s.SupplierCode + " " + s.SupplierName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SelectListItem>> GetFilprideEmployeeSupplierListAsyncById(string company, CancellationToken cancellationToken = default)
        {
            return await _db.Suppliers
                .Where(s => s.IsActive && s.Category == "Employee")
                .Where(GetCompanyFilter<Supplier>(company))
                .OrderBy(s => s.EmployeeNumber)
                .ThenBy(s => s.SupplierName)
                .Select(s => new SelectListItem
                {
                    Value = s.SupplierId.ToString(),
                    Text = string.IsNullOrWhiteSpace(s.EmployeeNumber)
                        ? s.SupplierName
                        : $"{s.EmployeeNumber} - {s.SupplierName}"
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SelectListItem>> GetFilprideTradeSupplierListAsyncById(string company, CancellationToken cancellationToken = default)
        {
            return await _db.Suppliers
                .OrderBy(s => s.SupplierCode)
                .Where(s => s.IsActive && s.Category == "Trade")
                .Where(GetCompanyFilter<Supplier>(company))
                .Select(s => new SelectListItem
                {
                    Value = s.SupplierId.ToString(),
                    Text = s.SupplierCode + " " + s.SupplierName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SelectListItem>> GetFilprideNonTradeSupplierListAsyncById(string company, CancellationToken cancellationToken = default)
        {
            return await _db.Suppliers
                .OrderBy(s => s.SupplierName)
                .Where(s => s.IsActive && s.Category == "Non-Trade" || s.Category == "Employee")
                .Where(GetCompanyFilter<Supplier>(company))
                .Select(s => new SelectListItem
                {
                    Value = s.SupplierId.ToString(),
                    Text = s.SupplierName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SelectListItem>> GetFilprideCommissioneeListAsyncById(string company, CancellationToken cancellationToken = default)
        {
            return await _db.Suppliers
                .OrderBy(s => s.SupplierCode)
                .Where(s => s.IsActive && s.Category == "Commissionee")
                .Where(GetCompanyFilter<Supplier>(company))
                .Select(s => new SelectListItem
                {
                    Value = s.SupplierId.ToString(),
                    Text = s.SupplierCode + " " + s.SupplierName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SelectListItem>> GetFilprideHaulerListAsyncById(string company, CancellationToken cancellationToken = default)
        {
            return await _db.Suppliers
                .OrderBy(s => s.SupplierCode)
                .Where(s => s.IsActive && s.Company == company && s.Category == "Hauler")
                .Where(GetCompanyFilter<Supplier>(company))
                .Select(s => new SelectListItem
                {
                    Value = s.SupplierId.ToString(),
                    Text = s.SupplierCode + " " + s.SupplierName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SelectListItem>> GetFilprideBankAccountListById(string company, CancellationToken cancellationToken = default)
        {
            return await _db.BankAccounts
                .Where(GetCompanyFilter<BankAccount>(company))
                .OrderBy(b => b.AccountNo)
                .Select(ba => new SelectListItem
                {
                    Value = ba.BankAccountId.ToString(),
                    Text = ba.Bank + " " + ba.AccountNo + " " + ba.AccountName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SelectListItem>> GetDistinctFilpridePickupPointListById(string companyClaims, CancellationToken cancellationToken = default)
        {
            return await _db.PickUpPoints
                .Where(GetCompanyFilter<PickUpPoint>(companyClaims))
                .GroupBy(p => p.Depot)
                .OrderBy(g => g.Key)
                .Select(g => new SelectListItem
                {
                    Value = g.First().PickUpPointId.ToString(),
                    Text = g.Key // g.Key is the Depot name
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SelectListItem>> GetFilprideServiceListById(string companyClaims, CancellationToken cancellationToken = default)
        {
            return await _db.Services
                .OrderBy(s => s.Name)
                .Where(GetCompanyFilter<Service>(companyClaims))
                .Select(s => new SelectListItem
                {
                    Value = s.ServiceId.ToString(),
                    Text = s.Name
                })
                .ToListAsync(cancellationToken);
        }

        #endregion

        public async Task<List<SelectListItem>> GetProductListAsyncByCode(CancellationToken cancellationToken = default)
        {
            return await _db.Products
                .OrderBy(p => p.ProductCode)
                .Where(p => p.IsActive)
                .Select(p => new SelectListItem
                {
                    Value = p.ProductCode,
                    Text = p.ProductCode + " " + p.ProductName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SelectListItem>> GetProductListAsyncById(CancellationToken cancellationToken = default)
        {
            return await _db.Products
                .OrderBy(p => p.ProductCode)
                .Where(p => p.IsActive)
                .Select(p => new SelectListItem
                {
                    Value = p.ProductId.ToString(),
                    Text = p.ProductCode + " " + p.ProductName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SelectListItem>> GetChartOfAccountListAsyncById(CancellationToken cancellationToken = default)
        {
            return await _db.ChartOfAccounts
                .Where(coa => !coa.HasChildren)
                .OrderBy(coa => coa.AccountNumber)
                .Select(s => new SelectListItem
                {
                    Value = s.AccountId.ToString(),
                    Text = s.AccountNumber + " " + s.AccountName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SelectListItem>> GetChartOfAccountListAsyncByNo(CancellationToken cancellationToken = default)
        {
            return await _db.ChartOfAccounts
                .Where(coa => !coa.HasChildren)
                .OrderBy(coa => coa.AccountNumber)
                .Select(s => new SelectListItem
                {
                    Value = s.AccountNumber,
                    Text = $"({s.AccountType}) {s.AccountNumber} {s.AccountName}"
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SelectListItem>> GetChartOfAccountListAsyncByAccountTitle(CancellationToken cancellationToken = default)
        {
            return await _db.ChartOfAccounts
                .Where(coa => !coa.HasChildren)
                .OrderBy(coa => coa.AccountNumber)
                .Select(s => new SelectListItem
                {
                    Value = s.AccountNumber + " " + s.AccountName,
                    Text = $"({s.AccountType}) {s.AccountNumber} {s.AccountName}"
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SelectListItem>> GetCompanyListAsyncByName(CancellationToken cancellationToken = default)
        {
            return await _db.Companies
                .OrderBy(c => c.CompanyCode)
                .Where(c => c.IsActive)
                .Select(c => new SelectListItem
                {
                    Value = c.CompanyName,
                    Text = c.CompanyCode + " " + c.CompanyName
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<List<SelectListItem>> GetCompanyListAsyncById(CancellationToken cancellationToken = default)
        {
            return await _db.Companies
                .OrderBy(c => c.CompanyCode)
                .Where(c => c.IsActive)
                .Select(c => new SelectListItem
                {
                    Value = c.CompanyId.ToString(),
                    Text = c.CompanyCode + " " + c.CompanyName
                })
                .ToListAsync(cancellationToken);
        }
    }
}

