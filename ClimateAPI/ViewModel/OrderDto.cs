using CCDbApi.Model;

namespace CCDbApi.ViewModel
{
    public class OrderDto
    {
        public string? Id {  get; set; }
        public string OrderNo { get; set; } = string.Empty;

        public string? UserId { get; set; }

        public string CustomerName { get; set; }
        public string? Email { get; set; }

        public string? Title { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public string PropertyAddress { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderDetailDto>OrderDetailDtos { get; set; }
    }
    public class OrderDetailDto
    {
        public string? Id { get; set; } 
        public string PublicationId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal Price { get; set; }
    }

}
