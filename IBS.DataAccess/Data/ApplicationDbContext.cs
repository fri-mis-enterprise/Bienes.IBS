using IBS.Models;
using IBS.Models.Bienes;
using IBS.Models.Filpride;
using IBS.Models.Filpride.AccountsPayable;
using IBS.Models.Filpride.AccountsReceivable;
using IBS.Models.Filpride.Books;
using IBS.Models.Filpride.Integrated;
using IBS.Models.Filpride.MasterFile;
using IBS.Models.MasterFile;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IBS.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSnakeCaseNamingConvention();
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<LogMessage> LogMessages { get; set; }

        public DbSet<AppSetting> AppSettings { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<UserNotification> UserNotifications { get; set; }

        public DbSet<HubConnection> HubConnections { get; set; }

        public DbSet<PostedPeriod> PostedPeriods { get; set; }

        public DbSet<DepartmentAccess> DepartmentAccesses { get; set; }

        public DbSet<LockedPeriodAdjustment> LockedPeriodAdjustments { get; set; }

        #region--FILPRIDE

        public DbSet<CustomerOrderSlip> CustomerOrderSlips { get; set; }

        public DbSet<DeliveryReceipt> DeliveryReceipts { get; set; }

        public DbSet<DeliveryReceiptDetail> DeliveryReceiptDetails { get; set; }

        public DbSet<Freight> Freights { get; set; }

        public DbSet<AuthorityToLoad> AuthorityToLoads { get; set; }

        public DbSet<COSAppointedSupplier> CosAppointedSuppliers { get; set; }

        public DbSet<POActualPrice> PoActualPrices { get; set; }

        public DbSet<CustomerBranch> CustomerBranches { get; set; }

        public DbSet<BookAtlDetail> BookAtlDetails { get; set; }

        public DbSet<MonthlyNibit> MonthlyNibits { get; set; }

        public DbSet<GLPeriodBalance> GlPeriodBalances { get; set; }

        public DbSet<GLSubAccountBalance> GlSubAccountBalances { get; set; }

        public DbSet<ProvisionalReceipt> ProvisionalReceipts { get; set; }

        #region--Master File

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<PickUpPoint> PickUpPoints { get; set; }

        public DbSet<Terms> Terms { get; set; }

        #endregion

        #endregion

        #region --BIENES

        public DbSet<Placement> Placements { get; set; }

        #endregion

        #region --Master File Entity

        public DbSet<Company> Companies { get; set; }

        public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        public DbSet<Product> Products { get; set; }

        #endregion --Master File Entities

        #region AAS Migration

        #region Accounts Receivable
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<CollectionReceipt> CollectionReceipts { get; set; }
        public DbSet<CreditMemo> CreditMemos { get; set; }
        public DbSet<DebitMemo> DebitMemos { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<ServiceInvoice> ServiceInvoices { get; set; }
        public DbSet<Offsettings> Offsettings { get; set; }
        public DbSet<CollectionReceiptDetail> CollectionReceiptDetails { get; set; }
        #endregion

        #region Accounts Payable

        public DbSet<CheckVoucherHeader> CheckVoucherHeaders { get; set; }
        public DbSet<CheckVoucherDetail> CheckVoucherDetails { get; set; }
        public DbSet<JournalVoucherHeader> JournalVoucherHeaders { get; set; }
        public DbSet<JournalVoucherDetail> JournalVoucherDetails { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<ReceivingReport> ReceivingReports { get; set; }

        public DbSet<MultipleCheckVoucherPayment> MultipleCheckVoucherPayments { get; set; }

        public DbSet<CVTradePayment> CvTradePayments { get; set; }

        public DbSet<JvAmortizationSetting> JvAmortizationSettings { get; set; }

        #endregion

        #region Books

        public DbSet<GeneralLedgerBook> GeneralLedgerBooks { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<AuditTrail> AuditTrails { get; set; }

        #endregion

        #endregion

        #region--Fluent API Implementation

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region-- Master File

            // Company
            builder.Entity<Company>(c =>
            {
                c.HasIndex(c => c.CompanyCode).IsUnique();
                c.HasIndex(c => c.CompanyName).IsUnique();
            });

            // Product
            builder.Entity<Product>(p =>
            {
                p.HasIndex(p => p.ProductCode).IsUnique();
                p.HasIndex(p => p.ProductName).IsUnique();
            });

            #endregion

            #region--Chart Of Account
            builder.Entity<ChartOfAccount>(coa =>
            {
                coa.HasIndex(coa => coa.AccountNumber).IsUnique();
                coa.HasIndex(coa => coa.AccountName);
                coa.HasQueryFilter(coa => !coa.IsHidden);
            });
            #endregion

            #region--Filpride

            builder.Entity<CustomerOrderSlip>(cos =>
            {
                cos.HasIndex(cos => new
                {
                    cos.CustomerOrderSlipNo,
                    cos.Company
                })
                .IsUnique();

                cos.HasIndex(cos => cos.Date);

                cos.HasOne(cos => cos.PurchaseOrder)
                    .WithMany()
                    .HasForeignKey(cos => cos.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                cos.HasOne(cos => cos.Customer)
                    .WithMany()
                    .HasForeignKey(cos => cos.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                cos.HasOne(cos => cos.Commissionee)
                    .WithMany()
                    .HasForeignKey(cos => cos.CommissioneeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<DeliveryReceipt>(dr =>
            {
                dr.HasIndex(dr => new
                {
                    dr.DeliveryReceiptNo,
                    dr.Company
                })
                .IsUnique();

                dr.HasIndex(dr => dr.Date);

                dr.HasOne(dr => dr.CustomerOrderSlip)
                    .WithMany(cos => cos.DeliveryReceipts)
                    .HasForeignKey(dr => dr.CustomerOrderSlipId)
                    .OnDelete(DeleteBehavior.Restrict);

                dr.HasOne(dr => dr.Commissionee)
                    .WithMany()
                    .HasForeignKey(dr => dr.CommissioneeId)
                    .OnDelete(DeleteBehavior.Restrict);

                dr.HasOne(dr => dr.Customer)
                    .WithMany()
                    .HasForeignKey(dr => dr.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                dr.HasOne(dr => dr.Hauler)
                    .WithMany()
                    .HasForeignKey(dr => dr.HaulerId)
                    .OnDelete(DeleteBehavior.Restrict);

                dr.HasOne(dr => dr.AuthorityToLoad)
                    .WithMany()
                    .HasForeignKey(dr => dr.AuthorityToLoadId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<DeliveryReceiptDetail>(d =>
            {
                d.HasOne(d => d.DeliveryReceipt)
                    .WithMany(dr => dr.Details)
                    .HasForeignKey(d => d.DeliveryReceiptId)
                    .OnDelete(DeleteBehavior.Cascade);

                d.HasOne(d => d.CustomerOrderSlip)
                    .WithMany()
                    .HasForeignKey(d => d.CustomerOrderSlipId)
                    .OnDelete(DeleteBehavior.Restrict);

                d.HasOne(d => d.PurchaseOrder)
                    .WithMany()
                    .HasForeignKey(d => d.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                d.HasOne(d => d.AuthorityToLoad)
                    .WithMany()
                    .HasForeignKey(d => d.AuthorityToLoadId)
                    .OnDelete(DeleteBehavior.Restrict);

                d.HasOne(d => d.Product)
                    .WithMany()
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<COSAppointedSupplier>(a =>
            {
                a.HasOne(a => a.PurchaseOrder)
                    .WithMany()
                    .HasForeignKey(a => a.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                a.HasOne(a => a.CustomerOrderSlip)
                    .WithMany(cos => cos.AppointedSuppliers)
                    .HasForeignKey(a => a.CustomerOrderSlipId)
                    .OnDelete(DeleteBehavior.Restrict);

                a.HasOne(a => a.Supplier)
                    .WithMany()
                    .HasForeignKey(a => a.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<POActualPrice>(p =>
            {
                p.HasOne(p => p.PurchaseOrder)
                    .WithMany(po => po.ActualPrices)
                    .HasForeignKey(p => p.PurchaseOrderId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<CustomerBranch>(b =>
            {
                b.HasOne(b => b.Customer)
                    .WithMany(c => c.Branches)
                    .HasForeignKey(b => b.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<BookAtlDetail>(b =>
            {
                b.HasOne(b => b.Header)
                    .WithMany(b => b.Details)
                    .HasForeignKey(b => b.AuthorityToLoadId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(b => b.CustomerOrderSlip)
                    .WithMany()
                    .HasForeignKey(b => b.CustomerOrderSlipId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(b => b.AppointedSupplier)
                    .WithMany()
                    .HasForeignKey(b => b.AppointedId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(b => b.Supplier)
                    .WithMany()
                    .HasForeignKey(b => b.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<AuthorityToLoad>(b =>
            {
                b.HasOne(b => b.Supplier)
                    .WithMany()
                    .HasForeignKey(b => b.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasIndex(b => new
                {
                    b.AuthorityToLoadNo,
                    b.Company
                })
                .IsUnique();
            });

            builder.Entity<MonthlyNibit>(n =>
            {
                n.HasQueryFilter(x => x.IsValid);
                n.HasIndex(n => n.Company);
                n.HasIndex(n => n.Month);
                n.HasIndex(n => n.Year);
            });

            #region-- Master File

            // Customer
            builder.Entity<Customer>(c =>
            {
                c.HasIndex(c => c.CustomerCode);
                c.HasIndex(c => c.CustomerName);
            });

            // Supplier
            builder.Entity<Supplier>(s =>
            {
                s.HasIndex(s => s.SupplierCode);
                s.HasIndex(s => s.SupplierName);
            });

            // PickUpPoint
            builder.Entity<PickUpPoint>(p =>
            {
                p.HasIndex(p => p.Company);

                p.HasOne(p => p.Supplier)
                    .WithMany()
                    .HasForeignKey(p => p.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<GLPeriodBalance>(b =>
            {
                b.HasQueryFilter(x => x.IsValid);

                b.HasOne(a => a.Account)
                    .WithMany(c => c.Balances)
                    .HasForeignKey(a => a.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<GLSubAccountBalance>(b =>
            {
                b.HasQueryFilter(x => x.IsValid);

                b.HasOne(a => a.Account)
                    .WithMany(c => c.SubAccountBalances)
                    .HasForeignKey(a => a.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region -- Accounts Receivable --

            #region -- Sales Invoice --

            builder.Entity<SalesInvoice>(si =>
            {
                si.HasOne(si => si.Product)
                .WithMany()
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

                si.HasOne(si => si.Customer)
                .WithMany()
                .HasForeignKey(si => si.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

                si.HasOne(si => si.PurchaseOrder)
                .WithMany()
                .HasForeignKey(si => si.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);

                si.HasIndex(si => new
                {
                    si.SalesInvoiceNo,
                    si.Company
                })
                .IsUnique();
            });

            #endregion -- Sales Invoice --

            #region -- Service Invoice --

            builder.Entity<ServiceInvoice>(sv =>
            {
                sv.HasOne(sv => sv.Customer)
                .WithMany()
                .HasForeignKey(sv => sv.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

                sv.HasOne(sv => sv.Service)
                .WithMany()
                .HasForeignKey(sv => sv.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

                sv.HasOne(sv => sv.DeliveryReceipt)
                    .WithMany()
                    .HasForeignKey(sv => sv.DeliveryReceiptId)
                    .OnDelete(DeleteBehavior.Restrict);

                sv.HasIndex(sv => new
                {
                    sv.ServiceInvoiceNo,
                    sv.Company
                })
                .IsUnique();
            });

            #endregion -- Service Invoice --

            #region -- Collection Receipt --

            builder.Entity<CollectionReceipt>(cr =>
            {
                cr.HasOne(cr => cr.SalesInvoice)
                .WithMany()
                .HasForeignKey(cr => cr.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

                cr.HasOne(cr => cr.ServiceInvoice)
                .WithMany()
                .HasForeignKey(cr => cr.ServiceInvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

                cr.HasOne(cr => cr.Customer)
                .WithMany()
                .HasForeignKey(cr => cr.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

                cr.HasOne(cr => cr.BankAccount)
                    .WithMany()
                    .HasForeignKey(cr => cr.BankId)
                    .OnDelete(DeleteBehavior.Restrict);

                cr.HasIndex(cr => new
                {
                    cr.CollectionReceiptNo,
                    cr.Company
                })
                .IsUnique();
            });

            builder.Entity<CollectionReceiptDetail>(crd =>
            {
                crd.HasOne(d => d.CollectionReceipt)
                    .WithMany(d => d.ReceiptDetails)
                    .HasForeignKey(d => d.CollectionReceiptId)
                    .OnDelete(DeleteBehavior.Restrict);

                crd.HasIndex(d => d.InvoiceNo);

                crd.HasIndex(d => d.CollectionReceiptNo);
            });

            builder.Entity<ProvisionalReceipt>(pr =>
            {
                pr.HasOne(p => p.Supplier)
                    .WithMany()
                    .HasForeignKey(p => p.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);

                pr.HasOne(p => p.BankAccount)
                    .WithMany()
                    .HasForeignKey(p => p.BankId)
                    .OnDelete(DeleteBehavior.Restrict);

                pr.HasIndex(d => new
                {
                    d.SeriesNumber,
                    d.Company
                }).IsUnique();
            });

            #endregion -- Collection Receipt --

            #region -- Debit Memo --

            builder.Entity<DebitMemo>(dm =>
            {
                dm.HasOne(dm => dm.SalesInvoice)
                .WithMany()
                .HasForeignKey(dm => dm.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

                dm.HasOne(dm => dm.ServiceInvoice)
                .WithMany()
                .HasForeignKey(dm => dm.ServiceInvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

                dm.HasIndex(dm => new
                {
                    dm.DebitMemoNo,
                    dm.Company
                })
                .IsUnique();
            });

            #endregion -- Debit Memo --

            #region -- Credit Memo --

            builder.Entity<CreditMemo>(cm =>
            {
                cm.HasOne(cm => cm.SalesInvoice)
                .WithMany()
                .HasForeignKey(cm => cm.SalesInvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

                cm.HasOne(cm => cm.ServiceInvoice)
                .WithMany()
                .HasForeignKey(cm => cm.ServiceInvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

                cm.HasIndex(cm => new
                {
                    cm.CreditMemoNo,
                    cm.Company
                })
                .IsUnique();
            });

            #endregion -- Credit Memo --

            #endregion -- Accounts Receivable --

            #region -- Accounts Payable --

            #region -- Purchase Order --

            builder.Entity<PurchaseOrder>(po =>
            {
                po.HasOne(po => po.Supplier)
                .WithMany()
                .HasForeignKey(po => po.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

                po.HasOne(po => po.Product)
                .WithMany()
                .HasForeignKey(po => po.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

                po.HasOne(po => po.Customer)
                .WithMany()
                .HasForeignKey(po => po.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

                po.HasOne(po => po.PickUpPoint)
                    .WithMany()
                    .HasForeignKey(po => po.PickUpPointId)
                    .OnDelete(DeleteBehavior.Restrict);

                po.HasIndex(po => new
                {
                    po.PurchaseOrderNo,
                    po.Company
                })
                .IsUnique();
            });

            #endregion -- Purchase Order --

            #region -- Receving Report --

            builder.Entity<ReceivingReport>(rr =>
            {
                rr.HasOne(rr => rr.PurchaseOrder)
                .WithMany(po => po.ReceivingReports)
                .HasForeignKey(rr => rr.POId)
                .OnDelete(DeleteBehavior.Restrict);

                rr.HasOne(rr => rr.DeliveryReceipt)
                .WithMany()
                .HasForeignKey(rr => rr.DeliveryReceiptId)
                .OnDelete(DeleteBehavior.Restrict);

                rr.HasIndex(rr => new
                {
                    rr.ReceivingReportNo,
                    rr.Company
                })
                .IsUnique();
            });

            #endregion -- Receving Report --

            #region -- Check Voucher Header --

            builder.Entity<CheckVoucherHeader>(cv =>
            {
                cv.HasOne(cv => cv.Supplier)
                .WithMany()
                .HasForeignKey(cv => cv.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

                cv.HasOne(cv => cv.BankAccount)
                .WithMany()
                .HasForeignKey(cv => cv.BankId)
                .OnDelete(DeleteBehavior.Restrict);

                cv.HasIndex(cv => new
                {
                    cv.CheckVoucherHeaderNo,
                    cv.Company
                })
                .IsUnique();
            });

            #endregion -- Check Voucher --

            #region -- Check Voucher Details --

            builder.Entity<CheckVoucherDetail>(cv =>
            {
                cv.HasOne(cv => cv.CheckVoucherHeader)
                    .WithMany(cv => cv.Details)
                    .HasForeignKey(cv => cv.CheckVoucherHeaderId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion -- Check Voucher Details --

            #region -- Check Voucher Trade Payment --

            builder.Entity<CVTradePayment>(cv =>
            {
                cv.HasOne(cv => cv.CV)
                    .WithMany()
                    .HasForeignKey(cv => cv.CheckVoucherId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion -- Check Voucher Trade Payment --

            #region -- Journal Voucher Header --

            builder.Entity<JournalVoucherHeader>(jv =>
            {
                jv.HasOne(jv => jv.CheckVoucherHeader)
                .WithMany()
                .HasForeignKey(jv => jv.CVId)
                .OnDelete(DeleteBehavior.Restrict);

                jv.HasIndex(jv => new
                {
                    jv.JournalVoucherHeaderNo,
                    jv.Company
                })
                .IsUnique();
            });

            #endregion -- Check Voucher --

            #region -- Journal Voucher Details --

            builder.Entity<JournalVoucherDetail>(jv =>
            {
                jv.HasOne(jv => jv.JournalVoucherHeader)
                    .WithMany(jv => jv.Details)
                    .HasForeignKey(jv => jv.JournalVoucherHeaderId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region

            builder.Entity<JvAmortizationSetting>(jv =>
            {
                jv.HasOne(jv => jv.JvHeader)
                    .WithMany()
                    .HasForeignKey(jv => jv.JvId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            #endregion

            #region -- Multiple Check Voucher Payment --

            builder.Entity<MultipleCheckVoucherPayment>(mcvp =>
            {
                mcvp.HasOne(mcvp => mcvp.CheckVoucherHeaderPayment)
                    .WithMany()
                    .HasForeignKey(mcvp => mcvp.CheckVoucherHeaderPaymentId)
                    .OnDelete(DeleteBehavior.Restrict);

                mcvp.HasOne(mcvp => mcvp.CheckVoucherHeaderInvoice)
                    .WithMany()
                    .HasForeignKey(mcvp => mcvp.CheckVoucherHeaderInvoiceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion -- Multiple Check Voucher Payment --

            #endregion -- Accounts Payable --

            #region-- Books --

            builder.Entity<GeneralLedgerBook>(gl =>
            {
                gl.HasOne(gl => gl.Account)
                    .WithMany()
                    .HasForeignKey(gl => gl.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #endregion

            #region--AppSettings

            builder.Entity<AppSetting>(a =>
            {
                a.HasIndex(a => a.SettingKey).IsUnique();
            });

            #endregion

            #region --Bienes

            builder.Entity<Placement>(placement =>
            {
                placement.HasOne(p => p.BankAccount)
                    .WithMany()
                    .HasForeignKey(p => p.BankId)
                    .OnDelete(DeleteBehavior.Restrict);

                placement.HasOne(p => p.Company)
                    .WithMany()
                    .HasForeignKey(p => p.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);

                placement.HasIndex(p => p.ControlNumber);
            });

            #endregion
        }

        #endregion
    }
}

