using CCDbApi.Model;
using CCDbApi.Service;
using CCDbApi.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace CCDbApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        // GET: api/Tags
        [HttpGet("getAllOrders")]
        public async Task<ActionResult<List<OrderDto>>> getAllOrder()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Order data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var orders = new List<OrderDto>();
                orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }

        }

        // GET: api/Tags
        [HttpGet("getAllOrdersByUser")]
        public async Task<ActionResult<List<OrderDto>>> getAllOrderByUser()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Order data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var orders = new List<OrderDto>();
                orders = await _orderService.GetAllOrdersByUserAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }

        }

        // GET: api/order/{id}
        [HttpGet("getOrder/{id}")]
        public async Task<ActionResult<OrderDto>> getOrder(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Order data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var order = new Order();
                order = await _orderService.GetOrderAsync(id);
                var orderDetails = await _orderService.GetAllOrderDetailsAsync(id);
                return (new OrderDto
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

                    OrderDetailDtos = orderDetails.Select(d => new OrderDetailDto
                    {
                        Id = d.Id.ToString(),
                        PublicationId = d.PublicationId,
                        Quantity = d.Quantity,
                        Price = d.Price
                    }).ToList()
                });
              
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        // POST: api/Order
        [HttpPost("addOrUpdateOrder")]
        public async Task<ActionResult<Order>> addOrder(OrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Order data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var order = new Order();
                var orderDetails = new List<OrderDetail>();
                if (dto.OrderDetailDtos.Any())
                {
                    dto.OrderDetailDtos.ForEach(a =>
                    orderDetails.Add(new OrderDetail()
                    {
                        OrderId = order.Id.ToString(),
                        Price = a.Price,
                        PublicationId = a.PublicationId,
                        Quantity = a.Quantity,
                        CreatedBy = order.CreatedBy,
                        CreatedDate = order.CreatedDate,

                    }));

                }
                if (dto.Id == null)
                {
                    order = new Order()
                    {
                        CreatedBy = userId,
                        CreatedDate = DateTime.Now,
                        CustomerId = dto.CustomerId,
                        Description = dto.Description,
                        DueDate = dto.DueDate,
                        OrderDate = DateTime.Now,
                        OrderNo = dto.OrderNo,
                        PropertyAddress = dto.PropertyAddress,
                        Status = dto.Status,
                        Title = dto.Title,
                        TotalAmount = dto.TotalAmount,

                        UserId = userId
                    };

                    order = await _orderService.AddOrderAsync(order);
                    if (order == null)
                    {
                        return BadRequest("Failed to add Order data");
                    }
                    await _orderService.AddOrderDetailRangeAsync(orderDetails);
                }
                else
                {
                    order = await _orderService.GetOrderAsync(dto.Id);
                    if (order == null)
                    {
                        return BadRequest("No such Order found with this id");
                    }
                    order.CustomerId = dto.CustomerId;
                    order.Description = dto.Description;
                    order.DueDate = dto.DueDate;
                    order.OrderDate = DateTime.Now;
                    order.OrderNo = dto.OrderNo;
                    order.PropertyAddress = dto.PropertyAddress;
                    order.Status = dto.Status;
                    order.Title = dto.Title;
                    order.TotalAmount = dto.TotalAmount;
                    order.UpdatedBy = userId;
                    order.UpdatedDate = DateTime.Now;
                    order = await _orderService.UpdateOrderAsync(order);
                    if (order == null)
                    {
                        return BadRequest("Failed to update Order data");
                    }
                    await _orderService.UpdateOrderDetailRangeAsync(orderDetails);
                }


                return Ok(order);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

        [HttpPost("uploadAttachment/{orderId}")]
        public async Task<IActionResult> UploadOrderAttachment(
    string orderId,
    IFormFile file)
        {
            if (string.IsNullOrWhiteSpace(orderId))
            {
                return BadRequest(new
                {
                    message = "OrderId is required."
                });
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    message = "Please select a file."
                });
            }

            var order =
                await _orderService.GetOrderAsync(orderId);

            if (order == null)
            {
                return NotFound(new
                {
                    message = "Order not found."
                });
            }

            var userId =
                HttpContext.User.FindFirst("Id")?.Value;

            var uploadPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "uploads",
                "orders"
            );

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var extension =
                Path.GetExtension(file.FileName);

            var generatedFileName =
                $"{Guid.NewGuid()}{extension}";

            var filePath = Path.Combine(
                uploadPath,
                generatedFileName
            );

            await using (var stream = new FileStream(
                filePath,
                FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl =
                $"{Request.Scheme}://{Request.Host}/uploads/orders/{generatedFileName}";

            var attachment = new OrderAttachment
            {
                OrderId = orderId,

                FileName = Path.GetFileName(file.FileName),

                FilePath = filePath,

                DownloadUrl = fileUrl,

                CreatedBy = userId,

                CreatedDate = DateTime.Now
            };

            await _orderService.AddOrderAttachmentAsync(
                attachment
            );

            return Ok(new
            {
                message = "Attachment uploaded successfully.",

                attachment
            });
        }
        [HttpPost("addOrUpdatePayment")]
        public async Task<IActionResult> AddOrUpdatePayment(
    [FromBody] PaymentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid payment data."
                });
            }

            var userId =
                HttpContext.User.FindFirst("Id")?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var order =
                await _orderService.GetOrderAsync(dto.OrderId);

            if (order == null)
            {
                return NotFound(new
                {
                    message = "Order not found."
                });
            }

            Payment payment;

            if (string.IsNullOrWhiteSpace(dto.Id))
            {
                payment = new Payment
                {
                    OrderId = dto.OrderId,

                    PaymentNo = dto.PaymentNo,

                    Amount = dto.Amount,

                    PaymentMethod = dto.PaymentMethod,

                    PaymentStatus = dto.PaymentStatus,

                    TransactionId = dto.TransactionId,

                    PaymentDate = dto.PaymentDate == default
                        ? DateTime.Now
                        : dto.PaymentDate,

                    Remarks = dto.Remarks,

                    UserId = userId,

                    CreatedBy = userId,

                    CreatedDate = DateTime.Now
                };

                payment =
                    await _orderService.AddPaymentAsync(payment);
            }
            else
            {
                payment =
                    await _orderService.GetPaymentAsync(dto.Id);

                if (payment == null)
                {
                    return NotFound(new
                    {
                        message = "Payment not found."
                    });
                }

                payment.PaymentNo = dto.PaymentNo;
                payment.Amount = dto.Amount;
                payment.PaymentMethod = dto.PaymentMethod;
                payment.PaymentStatus = dto.PaymentStatus;
                payment.TransactionId = dto.TransactionId;
                payment.PaymentDate = dto.PaymentDate;
                payment.Remarks = dto.Remarks;

                payment.UpdatedBy = userId;
                payment.UpdatedDate = DateTime.Now;

                payment =
                    await _orderService.UpdatePaymentAsync(payment);
            }

            return Ok(payment);
        }
        [HttpPost("addOrUpdateInvoice")]
        public async Task<IActionResult> AddOrUpdateInvoice(
    [FromBody] InvoiceDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid invoice data."
                });
            }

            var userId =
                HttpContext.User.FindFirst("Id")?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var order =
                await _orderService.GetOrderAsync(dto.OrderId);

            if (order == null)
            {
                return NotFound(new
                {
                    message = "Order not found."
                });
            }

            Invoice invoice;

            if (string.IsNullOrWhiteSpace(dto.Id))
            {
                invoice = new Invoice
                {
                    OrderId = dto.OrderId,

                    InvoiceNo = dto.InvoiceNo,

                    SubTotal = dto.SubTotal,

                    Tax = dto.Tax,

                    Discount = dto.Discount,

                    TotalAmount = dto.TotalAmount,

                    InvoiceDate = dto.InvoiceDate == default
                        ? DateTime.Now
                        : dto.InvoiceDate,

                    DueDate = dto.DueDate,

                    UserId = userId,

                    CreatedBy = userId,

                    CreatedDate = DateTime.Now
                };

                invoice =
                    await _orderService.AddInvoiceAsync(invoice);
            }
            else
            {
                invoice =
                    await _orderService.GetInvoiceAsync(dto.Id);

                if (invoice == null)
                {
                    return NotFound(new
                    {
                        message = "Invoice not found."
                    });
                }

                invoice.InvoiceNo = dto.InvoiceNo;
                invoice.SubTotal = dto.SubTotal;
                invoice.Tax = dto.Tax;
                invoice.Discount = dto.Discount;
                invoice.TotalAmount = dto.TotalAmount;
                invoice.InvoiceDate = dto.InvoiceDate;
                invoice.DueDate = dto.DueDate;

                invoice.UpdatedBy = userId;
                invoice.UpdatedDate = DateTime.Now;

                invoice =
                    await _orderService.UpdateInvoiceAsync(invoice);
            }

            return Ok(invoice);
        }
        // PUT: api/Order/{id}
        [HttpDelete("deleteOrder/{id}")]
        public async Task<IActionResult> deleteOrder(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    message = "Invalid Order data.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }
            try
            {
                // Retrieve user from context
                var user = HttpContext.User;
                // Optionally retrieve user ID if needed
                var userId = user.FindFirst("Id")?.Value;
                var email = user.FindFirst("Email")?.Value;
                var tag = new Order();
                tag = await _orderService.GetOrderAsync(id);
                if (tag != null)
                {
                    tag = await _orderService.DeleteOrderAsync(tag);
                    if (tag != null)
                    {
                        return Ok("Successfully deleted");
                    }
                    return BadRequest("Failed to delete record");
                }
                else
                {
                    return BadRequest("No such data found");
                }
                return Ok(tag);
            }
            catch (Exception ex)
            {
                // Log ex here
                var problem = Problem(detail: "An unexpected error occurred.", title: "Server Error");
                return StatusCode(StatusCodes.Status500InternalServerError, problem);
            }
        }

    }
}
