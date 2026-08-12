using CCDbApi.Model;

namespace CCDbApi.ViewModel
{
    public class PaymentDto
    {
        public string? Id { get; set; }

        public string OrderId { get; set; } = string.Empty;

        public string PaymentNo { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public string? TransactionId { get; set; }

        public DateTime PaymentDate { get; set; }

        public string? Remarks { get; set; }
    }
    public class InvoiceDto
    {
        public string? Id { get; set; }

        public string OrderId { get; set; } = string.Empty;

        public string InvoiceNo { get; set; } = string.Empty;

        public decimal SubTotal { get; set; }

        public decimal Tax { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime? DueDate { get; set; }
    }

}
