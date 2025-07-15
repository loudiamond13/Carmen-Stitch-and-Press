using CSP.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Carmen_Stitch_and_Press.Areas.Admin.ViewModels
{
    public class OrderViewModel
    {
        public int? OrderId { get; set; }
        [Required]
        public string OrderName { get; set; }
        public DateTime? OrderDate { get; set; }
        public string Note { get; set; } = "";
        public decimal TotalAmount { get; set; } = 0;

        public bool? IsOpen { get; set; }
        public string? CreatedBy { get; set; }

        public decimal PaidAmount { get; set; } = 0;

        public decimal TotalDiscount { get; set; } = 0;

        public decimal TotalBalance { get; set; } = 0;
        public decimal TotalExpenses { get; set; } = 0;

        public virtual List<DiscountViewModel> Discounts { get; set; } = new List<DiscountViewModel>();

        public virtual List<Expense> Expenses { get; set; } = new List<Expense>();

        // public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
        public virtual List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
        public virtual List<PaymentViewModel> Payments { get; set; } = new List<PaymentViewModel>();

        //payment 
        public string PayerName { get; set; } = "";
        public string PaymentCreatedBy { get; set; } = "";
        public decimal PaymentAmount { get; set; } = 0;
        public string PayTo { get; set; } = "";

        //discount 
        public string? DiscountDesc { get; set; }
        public decimal DiscountAmt { get; set; } = 0;
    }
}
