using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Entities;
using OrderManagement.Services;

namespace OrderManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Sipariş Ekleme
        [HttpPost]
        public async Task<ActionResult> CreateOrder(Order order)
        {
            try
            {
                await _orderService.AddOrderAsync(order);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Sipariş Listesi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        // Sipariş Detay
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // Sipariş Silme
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }

}


//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using OrderManagement.Entities;

//namespace OrderManagement.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class OrdersController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;

//        public OrdersController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // Yeni Sipariş Ekleme
//        [HttpPost]
//        public async Task<ActionResult<Order>> CreateOrder(Order order)
//        {
//            var product = await _context.Products.FindAsync(order.ProductId);

//            if (product == null || product.StockQuantity < order.Quantity)
//            {
//                return BadRequest("Yeterli stok yok.");
//            }

//            product.StockQuantity -= order.Quantity;

//            _context.Orders.Add(order);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
//        }

//        // Siparişleri Listeleme
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
//        {
//            return await _context.Orders.Include(o => o.Product).ToListAsync();
//        }

//        // Sipariş Detayını Getirme
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Order>> GetOrder(int id)
//        {
//            var order = await _context.Orders.Include(o => o.Product)
//                                              .FirstOrDefaultAsync(o => o.Id == id);

//            if (order == null)
//            {
//                return NotFound();
//            }

//            return order;
//        }

//        // Sipariş Silme
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteOrder(int id)
//        {
//            var order = await _context.Orders.FindAsync(id);
//            if (order == null)
//            {
//                return NotFound();
//            }

//            _context.Orders.Remove(order);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }
//    }

//}
