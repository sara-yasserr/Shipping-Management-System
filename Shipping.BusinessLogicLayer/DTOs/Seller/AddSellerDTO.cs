using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.DTOs.Seller
{
    public class AddSellerDTO
    {
        public string StoreName { get; set; }
        public string Address { get; set; }
        public decimal CancelledOrderPercentage { get; set; }
        public int CityId { get; set; }
        public string UserId { get; set; }
    }
}
