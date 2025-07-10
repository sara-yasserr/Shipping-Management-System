using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.UnitOfWorks;
using Shipping.DataAccessLayer.Enum;
using System.Linq;

namespace Shipping.BusinessLogicLayer.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly UnitOfWork _unitOfWork;

        public DashboardService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DashboardDTO GetDashboardData()
        {
            var dashboardData = new DashboardDTO();

            // Get all orders that are not deleted
            var orders = _unitOfWork.OrderRepo.GetAll()
                .Where(o => !o.IsDeleted)
                .ToList();

            // Get order status counts
            dashboardData.OrderStatusCounts = new OrderStatusCountsDTO
            {
                New = orders.Count(o => o.Status == OrderStatus.Pending),
                Pending = orders.Count(o => o.Status == OrderStatus.AcceptedByDeliveryCompany),
                DeliveredToAgent = orders.Count(o => o.Status == OrderStatus.DeliveredToDeliveryMan),
                Delivered = orders.Count(o => o.Status == OrderStatus.Delivered),
                CancelledByReceiver = orders.Count(o => o.Status == OrderStatus.CanceledByCustomer),
                PartiallyDelivered = orders.Count(o => o.Status == OrderStatus.PartiallyDelivered),
                Postponed = orders.Count(o => o.Status == OrderStatus.Postponed),
                NotReachable = orders.Count(o => o.Status == OrderStatus.CanNotBeReached),
                RefusedWithPartialPayment = orders.Count(o => o.Status == OrderStatus.RejectWithPartiallyPaid),
                RefusedWithoutPayment = orders.Count(o => o.Status == OrderStatus.RejectWithoutPayment)
            };

            // Get counts
            dashboardData.EmployeesCount = _unitOfWork.EmployeeRepo.GetAll().Count();
            dashboardData.SellersCount = _unitOfWork.SellerRepo.GetAll().Count();
            dashboardData.DeliveryAgentsCount = _unitOfWork.DeliveryManRepo.GetAll().Count();

            // Get top cities by order count
            var cityActivity = orders
                .GroupBy(o => o.City)
                .Select(g => new CityActivityDTO
                {
                    CityName = g.Key?.Name ?? "Unknown",
                    OrdersCount = g.Count(),
                    Revenue = g.Sum(o => o.TotalCost)
                })
                .OrderByDescending(c => c.OrdersCount)
                .Take(5)
                .ToList();

            dashboardData.TopCities = cityActivity;

            // Get recent orders with navigation properties (last 5 orders only)
            var recentOrders = orders
                .OrderByDescending(o => o.CreationDate)
                .Take(5) // Changed from 10 to 5
                .Select(o => new RecentOrderDTO
                {
                    OrderID = o.Id,
                    CustomerName = o.CustomerName,
                    CustomerCityName = o.City?.Name ?? "",
                    SellerName = o.Seller?.StoreName ?? "",
                    DeliveryAgentName = o.DeliveryAgent?.User?.UserName ?? "",
                    BranchName = o.Branch?.Name ?? "",
                    CreationDate = o.CreationDate,
                    Status = o.Status.ToString(),
                    TotalCost = o.TotalCost,
                    CustomerPhone = o.CustomerPhone,
                    Address = o.Address
                })
                .ToList();

            dashboardData.RecentOrders = recentOrders;

            // Calculate additional metrics
            dashboardData.TotalOrders = orders.Count;
            dashboardData.DeliveredOrders = dashboardData.OrderStatusCounts.Delivered;
            dashboardData.PendingOrders = dashboardData.OrderStatusCounts.Pending + dashboardData.OrderStatusCounts.New;

            // Calculate revenue metrics
            dashboardData.TotalRevenue = orders.Sum(o => o.TotalCost);
            dashboardData.DeliveredOrdersRevenue = orders
                .Where(o => o.Status == OrderStatus.Delivered)
                .Sum(o => o.TotalCost);
            dashboardData.PendingOrdersRevenue = orders
                .Where(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.AcceptedByDeliveryCompany)
                .Sum(o => o.TotalCost);

            // Calculate success rate
            dashboardData.DeliverySuccessRate = dashboardData.TotalOrders > 0 
                ? Math.Round((double)dashboardData.DeliveredOrders / dashboardData.TotalOrders * 100, 1)
                : 0;

            // Set top performers
            dashboardData.TopPerformingCity = cityActivity.FirstOrDefault()?.CityName ?? "N/A";
            
            var topSeller = orders
                .GroupBy(o => o.Seller?.StoreName)
                .Where(g => !string.IsNullOrEmpty(g.Key))
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();
            dashboardData.TopSeller = topSeller?.Key ?? "N/A";

            var topAgent = orders
                .GroupBy(o => o.DeliveryAgent?.User?.UserName)
                .Where(g => !string.IsNullOrEmpty(g.Key))
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();
            dashboardData.TopDeliveryAgent = topAgent?.Key ?? "N/A";

            // Set customer satisfaction (placeholder - would be calculated from ratings)
            dashboardData.CustomerSatisfaction = 92.5;

            return dashboardData;
        }
    }
}