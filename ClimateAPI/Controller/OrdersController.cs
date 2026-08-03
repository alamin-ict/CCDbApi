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



        // PUT: api/Order/{id}
        [HttpPut("deleteOrder/{id}")]
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
