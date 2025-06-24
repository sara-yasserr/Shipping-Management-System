using AutoMapper;
using Shipping.BusinessLogicLayer.DTOs.BranchDTOs;
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
                    dest.Governorate = src.Governorate.Name;
                });

            CreateMap<AddBranch, Branch>().AfterMap((src, dest) =>
            {
                dest.CreationDate = DateTime.Now;
            });
            #endregion
        }
    }
}
