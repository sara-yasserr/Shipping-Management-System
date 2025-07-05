using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.GovernorateDTOs;
using Shipping.BusinessLogicLayer.Helper;

namespace Shipping.BusinessLogicLayer.Interfaces
{
    public interface IGovernorateService
    {
        bool AddGovernorate(AddGovernorateDto dto);
        PagedResponse<ReadGovernorateDto> GetAll(PaginationDTO pagination);
        ReadGovernorateDto GetById(int id);
        bool EditGovernorate(int id, AddGovernorateDto dto);
        bool SoftDeleteGovernorate(int id);
        bool HardDeleteGovernorate(int id);
        void ActiveGovernorate(int id);
    }
}
