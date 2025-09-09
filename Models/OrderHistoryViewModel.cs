namespace Turbo_Food_Main.ViewModels
{
    public class OrderHistoryViewModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;

        public int MealID { get; set; }
        public string MealName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
