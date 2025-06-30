using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.DTOs.Seller
{
    public class UpdateSellerDTO 
    {
        public int Id { get; set; }  

       
        public string StoreName { get; set; }
        public string Address { get; set; }
        public decimal CancelledOrderPercentage { get; set; }
        public int CityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
