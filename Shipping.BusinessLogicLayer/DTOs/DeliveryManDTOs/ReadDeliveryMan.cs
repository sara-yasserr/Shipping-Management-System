using System.Collections.Generic;

namespace Shipping.BusinessLogicLayer.DTOs.DeliveryManDTOs
{
    public class ReadDeliveryMan
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? BranchName { get; set; }
        public int? BranchId { get; set; }
        public string? Cities { get; set; }
        public List<int>? CityIds { get; set; }

        public int? ActiveOrdersCount { get; set; }
        public bool IsDeleted {get; set;}
     
    }
} 