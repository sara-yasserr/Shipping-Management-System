using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.DTOs.Seller
{
    public class UpdateSellerDTO : AddSellerDTO
    {
        public int Id { get; set; }
    }
}
