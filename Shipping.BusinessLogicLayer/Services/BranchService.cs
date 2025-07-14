using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.BranchDTOs;
using Shipping.BusinessLogicLayer.Helper;
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
        public PagedResponse<ReadBranch> GetAllBranch(PaginationDTO pagination)
        {
            var branches = _unitOfWork.BranchRepo.GetAll();
            //var branches = _unitOfWork.BranchRepo.GetAll();
            var count = branches.Count();
            var pagedBranches = branches.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();
            var data = _mapper.Map<List<ReadBranch>>(pagedBranches);
            var result = new PagedResponse<ReadBranch>
            {
                Items = data,
                TotalCount = count,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)count / pagination.PageSize)
            };
            return result;
        }

        public List<ReadBranch> GetAllBranch()
        {
            var branches = _unitOfWork.BranchRepo.GetAll().Where(b => b.IsDeleted == false).ToList();
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

        public void ActivateBranch(int branchId)
        {
            var branch = _unitOfWork.BranchRepo.GetById(branchId);
            if (branch != null)
            {
                branch.IsDeleted = false; // Set IsDeleted to false to activate the branch
                _unitOfWork.BranchRepo.Update(branch);
                _unitOfWork.Save();
            }
            else
            {
                throw new Exception("Branch not found");
            }
        }
    }
}