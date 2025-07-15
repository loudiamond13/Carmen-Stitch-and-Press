namespace Carmen_Stitch_and_Press.Areas.Admin.ViewModels
{
    public class OrderItemViewModel
    {
        public int? OrderItemId { get; set; } = 0;

        public int OrderId { get; set; } = 0;

        public string Description { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public bool? IsDone { get; set; }

        public string? CreatedBy { get; set; }
    }
}
