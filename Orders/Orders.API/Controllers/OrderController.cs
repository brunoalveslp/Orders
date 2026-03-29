using Microsoft.AspNetCore.Mvc;
using Orders.Application.DTOs;
using Orders.Application.Services;
using Orders.API.Helpers;

namespace Orders.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _orderService.GetAllAsync();

            return Handlers.HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _orderService.GetByIdAsync(id);

            return Handlers.HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var result = await _orderService.CreateAsync(request);

            if (!result.IsSuccess)
            {
                return Handlers.HandleResult(result);
            }

            return Handlers.HandleCreateAtResult(result, nameof(Create), result.Value.Id.ToString());
        }
    }
}
