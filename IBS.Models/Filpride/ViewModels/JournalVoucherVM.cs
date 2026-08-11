using IBS.Models.Filpride.AccountsPayable;

namespace IBS.Models.Filpride.ViewModels
{
    public class JournalVoucherVM
    {
        public JournalVoucherHeader? Header { get; set; }
        public List<JournalVoucherDetail>? Details { get; set; }

        public bool IsAmortization { get; set; }
    }
}
