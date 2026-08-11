using IBS.Models.Filpride.AccountsReceivable;
using IBS.Models.Filpride.Integrated;

namespace IBS.Models.Filpride.ViewModels
{
    public class SalesReportViewModel
    {
        public SalesInvoice? SalesInvoice { get; set; }
        public DeliveryReceipt DeliveryReceipt { get; set; } = null!;

        public string SalesInvoiceNo => SalesInvoice?.SalesInvoiceNo ?? " ";
    }
}
