using CSP.Domain.Entities;
using CSP.Domain.Models;

namespace Carmen_Stitch_and_Press.Areas.Admin.ViewModels
{
    public class PaymentViewModel
    {
        public int PaymentId { get; set; } = 0;

        public int OrderId { get; set; } = 0;

        public DateTime? PaymentDate { get; set; }

        public string PayerName { get; set; }

        public string? CreatedBy { get; set; }

        public string? UpdatedBy { get; set; }

        public string? DeletedBy { get; set; }

        public bool? IsDeleted { get; set; }

        public decimal Amount { get; set; }

        public string PayTo { get; set; }

        public virtual Order? Order { get; set; }

        public List<CarmenStitchAndPressUserModel> carmenStitchAndPressUsers = new();

    }
}
