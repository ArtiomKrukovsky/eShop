using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Commands;
using Ordering.API.DTOs;
using Ordering.API.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOrderQueries _orderQueries;

        public OrdersController(IMediator mediator, IOrderQueries orderQueries)
        {
            _mediator = mediator;
            _orderQueries = orderQueries;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderSummaryDTO>>> GetOrdersAsync()
        {
            var orders = await _orderQueries.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrderAsync(int orderId)
        {
            var order = await _orderQueries.GetOrderAsync(orderId);
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateOrderAsync([FromBody] CreateOrderCommand createOrderCommand)
        {
           return await _mediator.Send(createOrderCommand);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
