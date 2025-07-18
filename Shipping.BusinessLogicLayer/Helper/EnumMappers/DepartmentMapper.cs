﻿using Shipping.DataAccessLayer.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Helper.EnumMappers
{
    public static class DepartmentMapper
    {
        public static Dictionary<Department, string> DepartmentDictionary = new Dictionary<Department, string>()
        {
            { Department.Employees, "Employees" },
            { Department.DeliveryAgents, "Delivery Agents" },
            { Department.Sellers, "Sellers" },
            { Department.Branches, "Branches" },
            { Department.Governorates, "Governorates" },
            { Department.Cities, "Cities" },
            { Department.Orders, "Orders" },
            { Department.GeneralSetting, "General Settings" }
        };

        public static string GetDepartmentName(Department department)
        { 
            return DepartmentDictionary.TryGetValue(department, out var name) ? name : "Unknown Department";
        }
    }
}
