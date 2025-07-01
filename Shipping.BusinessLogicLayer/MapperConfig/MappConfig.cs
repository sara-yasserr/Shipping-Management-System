using AutoMapper;
using Shipping.BusinessLogicLayer.DTOs.BranchDTOs;
using Shipping.BusinessLogicLayer.DTOs.City;
using Shipping.BusinessLogicLayer.DTOs.EmployeeDTOs;
using Shipping.BusinessLogicLayer.DTOs.GeneralSettingsDTOs;
using Shipping.BusinessLogicLayer.DTOs.GovernorateDTOs;

using Shipping.BusinessLogicLayer.DTOs.Seller;

using Shipping.BusinessLogicLayer.DTOs.PermissionDTOs;

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


            #region seller

            CreateMap<Seller, SellerDTO>().AfterMap((src, dest) =>
            {
                dest.CityName = src.City.Name;
                dest.Username = src.User.UserName;
                dest.FullName = src.User.FirstName + " "+src.User.LastName;
                dest.Email = src.User.Email;
                dest.PhoneNumber = src.User.PhoneNumber;

            }).ReverseMap();



            CreateMap<Seller, AddSellerDTO>().ReverseMap();
            CreateMap<Seller, UpdateSellerDTO>().ReverseMap();


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

            #region General Settings
            CreateMap<GeneralSetting, ReadGeneralSettingsDTO>()
            .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee.User.FirstName + " " + src.Employee.User.LastName)).ReverseMap();

            CreateMap<UpdateGeneralSettingsDTO, GeneralSetting>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => 1))
            .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(dest => dest.Fast, opt => opt.MapFrom(src => (decimal)src.Fast))
            .ForMember(dest => dest.Express, opt => opt.MapFrom(src => (decimal)src.Express))
            .ForMember(dest => dest.DefaultWeight, opt => opt.MapFrom(src => (decimal)src.DefaultWeight))
            .ForMember(dest => dest.ExtraPriceKg, opt => opt.MapFrom(src => (decimal)src.ExtraPriceKg))
            .ForMember(dest => dest.ExtraPriceVillage, opt => opt.MapFrom(src => (decimal)src.ExtraPriceVillage))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId)).ReverseMap();

            #endregion

            #region Role Permissions
            CreateMap<RolePermissions, PermissionDTO>()
            .ForMember(dest => dest.DepartmentName,
                       opt => opt.MapFrom(src => DepartmentMapper.GetDepartmentName(src.Department)))
            .ReverseMap()
            .ForMember(dest => dest.Department,
                       opt => opt.MapFrom(src => src.Department));

            #endregion
        }
    }
}
