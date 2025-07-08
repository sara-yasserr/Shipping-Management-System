using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.EnumDTOs;
using Shipping.BusinessLogicLayer.DTOs.OrderDTOs;
using Shipping.BusinessLogicLayer.Helper;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.BusinessLogicLayer.Services;
using Shipping.DataAccessLayer.Enum;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<ActionResult<PagedResponse<ReadOrderDTO>>> GetAllOrdersAsync([FromQuery] PaginationDTO pagination)
        {
            var orders = await _orderService.GetAllOrdersAsync(pagination);
            return Ok(orders);
        }

        [HttpGet("without-pagiantion")]
        public async Task<ActionResult<List<ReadOrderDTO>>> GetAllOrdersWithoutPaginationAsync()
        {
            var orders = await _orderService.GetAllWithoutPagination();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReadOrderDTO>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null) return NotFound();
            return Ok(order);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder( AddOrderDTO orderDTO)
        {
            if (orderDTO == null) return BadRequest("Order data is null");
            await _orderService.AddOrder(orderDTO);
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, UpdateOrderDTO updateOrderDTO)
        {
            if (id != updateOrderDTO.Id) return BadRequest("Order ID mismatch");
            try
            {
                await _orderService.UpdateOrder(updateOrderDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("soft/{id}")]
        public async Task<IActionResult> SoftDeleteOrder(int id)
        {
            try
            {
                await _orderService.SoftDeleteOrder(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        //hard delete
        //[HttpDelete("hard/{id}")]
        //public async Task<IActionResult> HardDeleteOrder(int id)
        //{
        //    try
        //    {
        //        _orderService.HardDeleteOrder(id);
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //}

        //Change Order Status
        [HttpPut("changeStatus/{orderId}")]
        public async Task<IActionResult> ChangeOrderStatus(int orderId, OrderStatus newStatus)
        {
            try
            {
                await _orderService.ChangeOrderStatus(orderId, newStatus);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        //Addign to Delivery Agent
        [HttpPut("assignDeliveryAgent")]
        public async Task<IActionResult> AssignDeliveryAgent(int orderId, int deliveryAgentId)
        {
            try
            {
                await _orderService.AssignDeliveryAgentToOrder(orderId , deliveryAgentId);
                return Content("Delivery Agent Assigned Successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        //Calculate Order Cost
        [HttpGet("calculateShippingCost")]
        public async Task<ActionResult<decimal>> CalculateShippingCost([FromQuery] OrderCostDTO order)
        {
            try
            {
                var cost = await _orderService.CalculateOrderShippingCost(order);
                return Ok(cost);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        //Get Orders by delivery Agent
        [HttpGet("GetOrdersByDeliveryAgent/{deliveryAgentId}")]
        public async Task<ActionResult<PagedResponse<ReadOrderDTO>>> GetOrdersByDeliveryAgentId(int deliveryAgentId, [FromQuery] PaginationDTO pagination)
        {
            var orders = await _orderService.GetOrdersByDeliveryAgentIdAsync(deliveryAgentId, pagination);
            return Ok(orders);
        }
        //Get Orders by Seller
        [HttpGet("GetOrdersBySeller/{sellerId}")]
        public async Task<ActionResult<PagedResponse<ReadOrderDTO>>> GetOrdersBySellerId(int sellerId, [FromQuery] PaginationDTO pagination)
        {
            var orders = await _orderService.GetOrdersBySellerIdAsync(sellerId, pagination);
            return Ok(orders);
        }


        //Get Orders by Status
        [HttpGet("GetByStatus")]
        public async Task<ActionResult<PagedResponse<ReadOrderDTO>>> GetOrdersByStatus(OrderStatus status, [FromQuery] PaginationDTO pagination)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status, pagination);
            return Ok(orders);
        }

        //Get Shipping Types
        [HttpGet("shippingTypes")]
        public async Task<ActionResult<List<ShippingAndOrderAndPaymentTypeDTO>>> GetShippingTypesAsync()
        {
            var shippingTypes = await _orderService.GetShippingTypesAsync();
            return Ok(shippingTypes);
        }

        //Get Orders Types
        [HttpGet("orderTypes")]
        public async Task<ActionResult<List<ShippingAndOrderAndPaymentTypeDTO>>> GetOrdersTypesAsync()
        {
            var orderTypes = await _orderService.GetOrdersTypesAsync();
            return Ok(orderTypes);
        }

        //Get Payment Types
        [HttpGet("paymentTypes")]
        public async Task<ActionResult<List<ShippingAndOrderAndPaymentTypeDTO>>> GetPaymentTypesAsync()
        {
            var paymentTypes = await _orderService.GetPaymentTypesAsync();
            return Ok(paymentTypes);
        }
        //Order Status Count
        [HttpGet("statusCount")]
        public async Task<ActionResult<EnumDTO>> GetOrderStatusCount(OrderStatus status)
        {
            var count = await _orderService.GetOrderStatusCount(status);
            return Ok(count);
        }

        //Get All Order Status Counts
        [HttpGet("allStatusCounts")]
        public async Task<ActionResult<PagedResponse<EnumDTO>>> GetAllOrderStatusCounts([FromQuery] PaginationDTO pagination)
        {
            var counts = await _orderService.GetAllOrderStatusCounts(pagination);
            return Ok(counts);
        }
        //Get Order Status Count for Seller
        [HttpGet("statusCountForSeller/{sellerId}")]
        public async Task<ActionResult<EnumDTO>> GetOrderStatusCountForSeller(int sellerId, OrderStatus status)
        {
            var count = await _orderService.GetOrderStatusCountForSeller(sellerId, status);
            return Ok(count);
        }

        //Get All Order Status Counts for Seller
        [HttpGet("allStatusCountsForSeller/{sellerId}")]
        public async Task<ActionResult<PagedResponse<EnumDTO>>> GetAllOrderStatusCountsForSeller(int sellerId, [FromQuery] PaginationDTO pagination)
        {
            var counts = await _orderService.GetAllOrderStatusCountsForSeller(sellerId, pagination);
            return Ok(counts);
        }
        //Get Order Status Count for Delivery Agent
        [HttpGet("statusCountForDeliveryAgent/{deliveryAgentId}")]
        public async Task<ActionResult<EnumDTO>> GetOrderStatusCountForDeliveryAgent(int deliveryAgentId, OrderStatus status)
        {
            var count = await _orderService.GetOrderStatusCountForDeliveryAgent(deliveryAgentId, status);
            return Ok(count);
        }
        //Get All Order Status Counts for Delivery Agent
        [HttpGet("allStatusCountsForDeliveryAgent/{deliveryAgentId}")]
        public async Task<ActionResult<PagedResponse<EnumDTO>>> GetAllOrderStatusCountsForDeliveryAgent(int deliveryAgentId, [FromQuery] PaginationDTO pagination)
        {
            var counts = await _orderService.GetAllOrderStatusCountsForDeliveryAgent(deliveryAgentId, pagination);
            return Ok(counts);
        }


    }
}
