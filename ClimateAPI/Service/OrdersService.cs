using CCDbApi.Model;
using CCDbApi.Repository;
using CCDbApi.ViewModel;

namespace CCDbApi.Service
{
    public interface IOrderService
    {
        Task<Order> AddOrderAsync(Order tags);
        Task<Order> UpdateOrderAsync(Order tags);
        Task<Order> DeleteOrderAsync(Order tags);
        Task<Order> GetOrderAsync(string id);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task<List<OrderDto>> GetAllOrdersByUserAsync(string userId);
        Task<List<OrderDetail>> GetAllOrderDetailsAsync(string orderId);

        Task<OrderAttachment> AddOrderAttachmentAsync(OrderAttachment orderAttachment);
        Task<List<OrderDetail>> AddOrderDetailRangeAsync(List<OrderDetail> tags);
        Task<List<OrderDetail>> UpdateOrderDetailRangeAsync(List<OrderDetail> tags);


        Task<Invoice> AddInvoiceAsync(Invoice tags);
        Task<Invoice> UpdateInvoiceAsync(Invoice tags);
        Task<Invoice> DeleteInvoiceAsync(Invoice tags);
        Task<Invoice> GetInvoiceAsync(string id);
        Task<Payment> AddPaymentAsync(Payment tags);
        Task<Payment> UpdatePaymentAsync(Payment tags);
        Task<Payment> GetPaymentAsync(string id);
    }
    public class OrdersService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly IOrderAttachmentRepository _attachmentRepo;
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly IPaymentRepository _paymentRepo;
        public OrdersService(IPaymentRepository payment, IOrderAttachmentRepository orderAttachment,
            IOrderDetailRepository orderDetail, IOrderRepository order, IInvoiceRepository invoiceRepository

            )
        {

            _attachmentRepo = orderAttachment;
            _invoiceRepo = invoiceRepository;
            _orderRepo = order;
            _orderDetailRepo = orderDetail;
            _paymentRepo = payment;

        }
        public async Task<Order> AddOrderAsync(Order Order)
        {
            var added = await _orderRepo.AddAsync(Order);
            if (added == 1)
            {
                return Order;
            }
            return null;
        }

        public async Task<Order> DeleteOrderAsync(Order Order)
        {
            var deleted = await _orderRepo.RemoveAsync(Order);
            if (deleted == 1)
            {
                var orderDetails = await _orderDetailRepo.FindAsync(a => a.OrderId == Order.Id.ToString());
                if (orderDetails != null)
                {
                    await _orderDetailRepo.RemoveRangeAsync(orderDetails);
                }
                return Order;
            }
            return null;
        }
        public async Task<Order> GetOrderAsync(string id)
        {
            var data = await _orderRepo.FindAsync(a => a.Id.ToString() == id);
            if (data == null)
            {
                return null;
            }
            return data.FirstOrDefault();
        }

        public async Task<Order> UpdateOrderAsync(Order Order)
        {
            var updated = await _orderRepo.UpdateAsync(Order);
            if (updated == 1)
            {
                return Order;
            }
            return null;
        }

        public async Task<OrderAttachment> AddOrderAttachmentAsync(OrderAttachment orderAttachment)
        {
            var added = await _attachmentRepo.AddAsync(orderAttachment);
            if (added == 1)
            {
                return orderAttachment;
            }
            return null;

        }
        public async Task<Payment> AddPaymentAsync(Payment Order)
        {
            var added = await _paymentRepo.AddAsync(Order);
            if (added == 1)
            {
                return Order;
            }
            return null;
        }

        public async Task<Payment> DeletePaymentAsync(Payment Order)
        {
            var deleted = await _paymentRepo.RemoveAsync(Order);
            if (deleted == 1)
            {
               
                return Order;
            }
            return null;
        }
        public async Task<Payment> GetPaymentAsync(string id)
        {
            var data = await _paymentRepo.FindAsync(a => a.Id.ToString() == id);
            if (data == null)
            {
                return null;
            }
            return data.FirstOrDefault();
        }
        
        public async Task<Payment> UpdatePaymentAsync(Payment Order)
        {
            var updated = await _paymentRepo.UpdateAsync(Order);
            if (updated == 1)
            {
                return Order;
            }
            return null;
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepo.GetAllAsync();

            if (!orders.Any())
                return new List<OrderDto>();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var details = await _orderDetailRepo.FindAsync(x => x.OrderId == order.Id.ToString());

                orderDtos.Add(new OrderDto
                {
                    Id = order.Id.ToString(),
                    OrderNo = order.OrderNo,
                    UserId = order.UserId,
                    CustomerName = order.CustomerName,
                    Email = order.Email,
                    Title = order.Title,
                    Description = order.Description,
                    PropertyAddress = order.PropertyAddress,
                    OrderDate = order.OrderDate,
                    DueDate = order.DueDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,

                    OrderDetailDtos = details.Select(d => new OrderDetailDto
                    {
                        Id = d.Id.ToString(),
                        PublicationId = d.PublicationId,
                        Quantity = d.Quantity,
                        Price = d.Price
                    }).ToList()
                });
            }

            return orderDtos;
        }
        public async Task<List<OrderDto>> GetAllOrdersByUserAsync(string userId)
        {
            var orders = await _orderRepo.FindAsync(a => a.UserId == userId);

            if (!orders.Any() || orders == null)
                return new List<OrderDto>();

            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var details = await _orderDetailRepo.FindAsync(x => x.OrderId == order.Id.ToString());

                orderDtos.Add(new OrderDto
                {
                    Id = order.Id.ToString(),
                    OrderNo = order.OrderNo,
                    UserId = order.UserId,
                    CustomerName = order.CustomerName,
                    Email = order.Email,
                    Title = order.Title,
                    Description = order.Description,
                    PropertyAddress = order.PropertyAddress,
                    OrderDate = order.OrderDate,
                    DueDate = order.DueDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,

                    OrderDetailDtos = details.Select(d => new OrderDetailDto
                    {
                        Id = d.Id.ToString(),
                        PublicationId = d.PublicationId,
                        Quantity = d.Quantity,
                        Price = d.Price
                    }).ToList()
                });
            }

            return orderDtos;
        }

        public async Task<Invoice> AddInvoiceAsync(Invoice Order)
        {
            var added = await _invoiceRepo.AddAsync(Order);
            if (added == 1)
            {
                return Order;
            }
            return null;
        }

        public async Task<Invoice> DeleteInvoiceAsync(Invoice Order)
        {
            var deleted = await _invoiceRepo.RemoveAsync(Order);
            if (deleted == 1)
            {
               
                return Order;
            }
            return null;
        }

        public async Task<Invoice> UpdateInvoiceAsync(Invoice Order)
        {
            var updated = await _invoiceRepo.UpdateAsync(Order);
            if (updated == 1)
            {
                return Order;
            }
            return null;
        }
        public async Task<Invoice> GetInvoiceAsync(string id)
        {
            var data = await _invoiceRepo.FindAsync(a => a.Id.ToString() == id);
            if (data == null)
            {
                return null;
            }
            return data.FirstOrDefault();
        }

        public async Task<List<OrderDetail>> GetAllOrderDetailsAsync(string orderId)
        {
            var data = await _orderDetailRepo.FindAsync(a => a.OrderId == orderId);
            if (data == null)
            {
                return null;
            }
            return data.ToList();
        }

        //public async Task<Order> GetOrderAsync(string id)
        //{
        //    var data = await _orderRepo.FindAsync(a => a.Id.ToString() == id);
        //    if (data == null)
        //    {
        //        return null;
        //    }
        //    return data.FirstOrDefault();
        //}

        public async Task<List<OrderDetail>> AddOrderDetailRangeAsync(List<OrderDetail> orderDetails)
        {
            try
            {
                await _orderDetailRepo.AddRangeAsync(orderDetails);

                return orderDetails;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<OrderDetail>> UpdateOrderDetailRangeAsync(List<OrderDetail> orderDetails)
        {
            var data = await GetAllOrderDetailsAsync(orderDetails.FirstOrDefault().OrderId);
            if (data != null)
            {
                await _orderDetailRepo.RemoveRangeAsync(data);
            }
            await AddOrderDetailRangeAsync(orderDetails);
            return orderDetails;
        }
    }
}
