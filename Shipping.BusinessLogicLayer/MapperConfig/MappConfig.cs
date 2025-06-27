using AutoMapper;
using Shipping.BusinessLogicLayer.DTOs.BranchDTOs;
using Shipping.BusinessLogicLayer.DTOs.City;
using Shipping.BusinessLogicLayer.DTOs.EmployeeDTOs;
using Shipping.BusinessLogicLayer.DTOs.GovernorateDTOs;
using Shipping.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.BusinessLogicLayer.Helper
{
    public class MappConfig : Profile
    {
        public MappConfig()
        {
            #region Branch
            CreateMap<Branch, ReadBranch>()
               .AfterMap((src, dest) =>
               {
                   dest.City = src.City.Name;

                   dest.DeliverAgents = src.DeliveryAgents.Select(d => d.User.UserName).ToList();
                   dest.Employees = src.Employees.Select(e => e.User.UserName).ToList();
               }).ReverseMap();

            CreateMap<AddBranch, Branch>().AfterMap((src, dest) =>
            {
                dest.CreationDate = DateTime.Now;
            });
            #endregion

            #region Governorate
            CreateMap<Governorate, ReadGovernorateDto>();
            CreateMap<AddGovernorateDto, Governorate>();
            #endregion

            #region Employee
            CreateMap<Employee, ReadEmployeeDTO>().AfterMap((src, dest) =>
            {
                if(src.Branch != null)
                {
                    dest.Branch = src.Branch.Name;
                }
                else
                {
                    dest.Branch = "General";
                }
                dest.Id = src.Id;
                dest.UserName = src.User.UserName;
                dest.Email = src.User.Email;
                dest.FirstName = src.User.FirstName;
                dest.LastName = src.User.LastName;
                dest.PhoneNumber = src.User.PhoneNumber;
                dest.CreatedAt = src.User.CreatedAt;
            });

            CreateMap<AddEmployeeDTO, Employee>().AfterMap((src, dest) =>
            {
                dest.BranchId = src.BranchId;
 
                dest.User = new ApplicationUser
                {
                    UserName = src.UserName,
                    Email = src.Email,
                    PasswordHash = src.Password,
                    FirstName = src.FirstName,
                    LastName = src.LastName,
                    PhoneNumber = src.PhoneNumber
                };
                dest.UserId = dest.User.Id;
            });
            #endregion

            #region City
            CreateMap<CreateCityDTO, City>();
            #endregion
        }
    }
}
