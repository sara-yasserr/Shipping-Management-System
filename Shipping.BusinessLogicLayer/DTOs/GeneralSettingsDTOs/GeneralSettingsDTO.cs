using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.DTOs.GeneralSettingsDTOs
{
    public record ReadGeneralSettingsDTO
    (
        int Id,
        decimal DefaultWeight,
        decimal ExtraPriceKg,
        decimal ExtraPriceVillage,
        decimal Fast,
        decimal Express,
        DateTime ModifiedAt,
        string Employee
    );
    public record UpdateGeneralSettingsDTO
    (
        decimal DefaultWeight,
        decimal ExtraPriceKg,
        decimal ExtraPriceVillage,
        decimal Fast,
        decimal Express,
        int EmployeeId
    );
}
