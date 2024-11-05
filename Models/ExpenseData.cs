namespace TextDataProcessing.Models
{
    
    public class ExpenseData
    {
        public string CostCentre { get; set; } = "UNKNOWN"; // Default to 'UNKNOWN'
        public decimal Total { get; set; }
        public decimal TotalExcludingTax { get; set; }
        public decimal SalesTax { get; set; }
        public string PaymentMethod { get; set; }
        public string Vendor { get; set; }
        public string Description { get; set; }
        public DateTime? Date { get; set; }
    }

}
