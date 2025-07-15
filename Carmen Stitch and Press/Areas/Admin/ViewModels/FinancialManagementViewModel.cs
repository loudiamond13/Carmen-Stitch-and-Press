using CSP.Domain.Entities;
using CSP.Domain.Models;

namespace Carmen_Stitch_and_Press.Areas.Admin.ViewModels
{
    public class FinancialManagementViewModel 
    {
        //index view
        public decimal AllOrdersTotal { get; set; } = 0;
        public decimal TotalReceivedMoney { get; set; } = 0;

        public decimal TotalExpenses { get; set; } = 0;
        public decimal TotalProfit { get; set; } = 0;

        public decimal Collectible { get; set; } = 0;
        public int AcceptedOrderQtyTotal { get; set; } = 0;

        //money transfer model
        public int MoneyTransfersId { get; set; } = 0;

        public string TransferFrom { get; set; } = "";

        public string TransferTo { get; set; } = "";

        public DateTime TransferDate { get; set; } = DateTime.Now;
        public string TransferDateString { get; set; } = "";

        public decimal TransferAmount { get; set; }

        public List<CarmenStitchAndPressUserModel> carmenStitchAndPressUserModels { get; set; } = new();
    }
}
