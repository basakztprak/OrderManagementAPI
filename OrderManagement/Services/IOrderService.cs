using OrderManagement.Entities;

namespace OrderManagement.Services
{
    public interface IOrderService
    {
        Task<Order> GetOrderAsync(int id);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
    }
}
