using Microsoft.EntityFrameworkCore;
using OrderManagement.Entities;
using OrderManagement.Repositories;

namespace OrderManagement.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _dbContext; // DbContext doğrudan enjekte edilecek

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, ApplicationDbContext dbContext)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _dbContext = dbContext; // DbContext burada kullanılacak
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            // Takip edilen nesneleri sıfırla (önceki nesneleri takip etmeyi bırak)
            DetachTrackedEntities();

            // Stok kontrolü
            var product = await _productRepository.GetByIdAsync(order.ProductId);
            if (product == null || product.StockQuantity < order.Quantity)
            {
                throw new Exception("Yeterli stok yok.");
            }

            // Stok miktarını güncelle
            product.StockQuantity -= order.Quantity;

            // Siparişi kaydet
            await _orderRepository.AddAsync(order);
            await _productRepository.UpdateAsync(product); // Ürün stoğunu güncelle
        }

        // **DbContext'te takip edilen nesneleri sıfırlama metodu**
        private void DetachTrackedEntities()
        {
            var entries = _dbContext.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added)
                .ToList();

            foreach (var entry in entries)
            {
                entry.State = EntityState.Detached;
            }
        }


        public async Task UpdateOrderAsync(Order order)
        {
            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteAsync(id);
        }
    }
}
