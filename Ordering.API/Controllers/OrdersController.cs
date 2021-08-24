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

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderSummaryDTO>>> GetOrdersAsync()
        {
            var orders = await _mediator.Send(new GetOrdersQuery());
            return Ok(orders);
        }

        [HttpGet("{orderId:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrderAsync(int orderId)
        {
            var order = await _mediator.Send(new GetOrderQuery(orderId));
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
