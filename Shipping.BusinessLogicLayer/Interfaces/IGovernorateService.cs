using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipping.BusinessLogicLayer.DTOs.GovernorateDTOs;

namespace Shipping.BusinessLogicLayer.Interfaces
{
    public interface IGovernorateService
    {
        bool AddGovernorate(AddGovernorateDto dto);
        List<ReadGovernorateDto> GetAll();
        ReadGovernorateDto GetById(int id);
        bool EditGovernorate(int id, AddGovernorateDto dto);
        bool DeleteGovernorate(int id);
    }
}
