namespace Carmen_Stitch_and_Press.Areas.Admin.ViewModels
{
    public class DiscountViewModel
    {
        public int OrderDiscountId { get; set; } = 0;

        public int OrderId { get; set; } = 0;

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public string? DiscountedBy { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
