using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.DTOs.Seller
{
    public class SellerDTO
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string CityName { get; set; }

        public string StoreName { get; set; }
        public string Address { get; set; }
        public decimal CancelledOrderPercentage { get; set; }


       
       
        
    }
}
