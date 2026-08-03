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


        Task<List<OrderDetail>> AddOrderDetailRangeAsync(List<OrderDetail> tags);
        Task<List<OrderDetail>> UpdateOrderDetailRangeAsync(List<OrderDetail> tags);
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
                var orderDetails = await GetAllOrderDetailsAsync(Order.Id.ToString());
                if (orderDetails != null)
                {
                    await _orderDetailRepo.RemoveRangeAsync(orderDetails);
                }
                return Order;
            }
            return null;
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
                    CustomerId = order.CustomerId,
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
                    CustomerId = order.CustomerId,
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



        public async Task<List<OrderDetail>> GetAllOrderDetailsAsync(string orderId)
        {
            var data = await _orderDetailRepo.FindAsync(a => a.OrderId == orderId);
            if (data == null)
            {
                return null;
            }
            return data.ToList();
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
