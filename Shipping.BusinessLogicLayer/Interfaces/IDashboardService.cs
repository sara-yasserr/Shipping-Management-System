using Shipping.BusinessLogicLayer.DTOs;

namespace Shipping.BusinessLogicLayer.Interfaces
{
    public interface IDashboardService
    {
        DashboardDTO GetDashboardData();
        OrderStatusCountsDTO GetOrderStatusCountsForDeliveryAgent(int deliveryAgentId);
        Shipping.DataAccessLayer.Models.DeliveryAgent? GetDeliveryAgentByUserId(string userId);
        OrderStatusCountsDTO GetOrderStatusCountsForSeller(int sellerId);
        Shipping.DataAccessLayer.Models.Seller? GetSellerByUserId(string userId);
    }
}