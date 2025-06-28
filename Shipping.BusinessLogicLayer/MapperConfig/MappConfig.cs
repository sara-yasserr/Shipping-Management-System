using AutoMapper;
using Shipping.BusinessLogicLayer.DTOs.BranchDTOs;
using Shipping.BusinessLogicLayer.DTOs.GovernorateDTOs;
using Shipping.BusinessLogicLayer.DTOs.Seller;
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
                   dest.Governorate = src.City.Governorate.Name;
               });

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

            }).ReverseMap();



            CreateMap<Seller, AddSellerDTO>().ReverseMap();
            CreateMap<Seller, UpdateSellerDTO>().ReverseMap();


            #endregion



            #region Governorate
            CreateMap<Governorate, ReadGovernorateDto>();
            CreateMap<AddGovernorateDto, Governorate>();
            #endregion
        }
    }
}
