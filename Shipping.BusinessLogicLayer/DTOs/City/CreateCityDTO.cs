using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.DTOs.City
{
    public class CreateCityDTO
    {
        public string Name { get; set; }
        public decimal NormalPrice { get; set; }
        public decimal PickupPrice { get; set; }
        public int GovernorateId { get; set; }
    }
}
