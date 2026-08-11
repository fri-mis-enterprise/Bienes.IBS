using IBS.Models.Filpride.AccountsPayable;
using IBS.Models.Filpride.MasterFile;

namespace IBS.Models.Filpride.ViewModels
{
    public class CheckVoucherVM
    {
        public CheckVoucherHeader? Header { get; set; }
        public List<CheckVoucherDetail>? Details { get; set; }

        public Supplier? Supplier { get; set; }
    }
}
