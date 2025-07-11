using System.Collections.Generic;

namespace Shipping.BusinessLogicLayer.DTOs
{
    public class DashboardDTO
    {
        public OrderStatusCountsDTO OrderStatusCounts { get; set; } = new OrderStatusCountsDTO();
        public int EmployeesCount { get; set; }
        public int SellersCount { get; set; }
        public int DeliveryAgentsCount { get; set; }
        public List<CityActivityDTO> TopCities { get; set; } = new List<CityActivityDTO>();
        public List<RecentOrderDTO> RecentOrders { get; set; } = new List<RecentOrderDTO>();
        public decimal TotalRevenue { get; set; }
        public decimal DeliveredOrdersRevenue { get; set; }
        public decimal PendingOrdersRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public int PendingOrders { get; set; }
        public double DeliverySuccessRate { get; set; }
        public double CustomerSatisfaction { get; set; }
        public string AverageDeliveryTime { get; set; } = "2.5 days";
        public string TopPerformingCity { get; set; } = "N/A";
        public string TopSeller { get; set; } = "N/A";
        public string TopDeliveryAgent { get; set; } = "N/A";
    }

    public class OrderStatusCountsDTO
    {
        public int New { get; set; }
        public int Pending { get; set; }
        public int DeliveredToAgent { get; set; }
        public int Delivered { get; set; }
        public int CancelledByReceiver { get; set; }
        public int PartiallyDelivered { get; set; }
        public int Postponed { get; set; }
        public int NotReachable { get; set; }
        public int RefusedWithPartialPayment { get; set; }
        public int RefusedWithoutPayment { get; set; }
    }

    public class CityActivityDTO
    {
        public string CityName { get; set; } = "";
        public int OrdersCount { get; set; }
        public decimal Revenue { get; set; }
    }

    public class RecentOrderDTO
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; } = "";
        public string CustomerCityName { get; set; } = "";
        public string SellerName { get; set; } = "";
        public string DeliveryAgentName { get; set; } = "";
        public string BranchName { get; set; } = "";
        public DateTime CreationDate { get; set; }
        public string Status { get; set; } = "";
        public decimal TotalCost { get; set; }
        public string CustomerPhone { get; set; } = "";
        public string Address { get; set; } = "";
    }
}