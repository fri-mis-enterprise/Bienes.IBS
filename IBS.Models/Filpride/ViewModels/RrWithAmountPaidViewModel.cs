using IBS.Models.Filpride.AccountsPayable;

namespace IBS.Models.Filpride.ViewModels
{
    public class RrWithAmountPaidViewModel
    {
        public ReceivingReport ReceivingReport { get; set; } = null!;
        public decimal AmountPaid { get; set; }
    }
}
