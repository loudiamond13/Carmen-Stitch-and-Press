using CSP.Domain.Models;

namespace Carmen_Stitch_and_Press.Areas.Admin.ViewModels
{
    public class ExpenseViewModel
    {
        public int? ExpensesId { get; set; }

        public int? OrderId { get; set; }

        public string Description { get; set; }

        public DateTime? SpendDate { get; set; }

        public bool? IsCompanyExpenses { get; set; }

        public decimal Amount { get; set; }

        public string PaidBy { get; set; } = "";

        public string OrderName { get; set; } = "";

        public string SpendDateString { get; set; } = "";

        public List<CarmenStitchAndPressUserModel> carmenStitchAndPressUsers { get; set; } = new();

        public string OrderTotalQty { get; set; } = "";

        public List<string> DistinctYears { get; set; } = new(); 
    }
}
