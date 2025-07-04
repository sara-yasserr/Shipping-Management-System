using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Shipping.BusinessLogicLayer.DTOs.BranchDTOs;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;

namespace Shipping.BusinessLogicLayer.Services
{
    public class BranchService : IBranchService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BranchService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public List<ReadBranch> GetAllBranch()
        {
            var branches = _unitOfWork.BranchRepo.GetAll().ToList();
            return _mapper.Map<List<ReadBranch>>(branches);
        }
        public Branch? GetBranchById(int id)
        {
            var branch = _unitOfWork.BranchRepo.GetById(id);
            if (branch == null)
            {
                return null;
            }
            return branch;
        }
        public ReadBranch? GetBranchDTOById(int id)
        {
            var branch = GetBranchById(id);
            return branch != null ? _mapper.Map<ReadBranch>(branch) : null;
        }
        public Branch AddBranch(AddBranch addBranchDTO)
        {
            var branch = _mapper.Map<Branch>(addBranchDTO);
            _unitOfWork.BranchRepo.Add(branch);
            _unitOfWork.Save();
            return branch;
        }
        public void UpdateBranch(AddBranch updateBranchDTO, Branch branch)
        {
            _mapper.Map(updateBranchDTO, branch);
            _unitOfWork.BranchRepo.Update(branch);
            _unitOfWork.Save();
        }
        public void SoftDelete(Branch branch)
        {
            branch.IsDeleted = true;
            _unitOfWork.BranchRepo.Update(branch);
            _unitOfWork.Save();
        }
        public void HardDelete(Branch branch)
        {
            _unitOfWork.BranchRepo.Delete(branch);
            _unitOfWork.Save();
        }
    }
}