using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.EmployeeDTOs;
using Shipping.BusinessLogicLayer.Helper;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;

namespace Shipping.BusinessLogicLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public EmployeeService(UnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public PagedResponse<ReadEmployeeDTO> GetAllEmployees(PaginationDTO pagination)
        {
            var employees = _unitOfWork.EmployeeRepo.GetAll();
            //var employees = _unitOfWork.EmployeeRepo.GetAll();
            
            var Count = employees.Count();
            var pagedEmployees = employees
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();
            var result = new PagedResponse<ReadEmployeeDTO>
            {
                Items = _mapper.Map<List<ReadEmployeeDTO>>(pagedEmployees),
                TotalCount = Count,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalPages = (int)Math.Ceiling((double)Count / pagination.PageSize)
            };

            return result;
        }
        public List<ReadEmployeeDTO> GetAllEmployee()
        {
           var employees = _unitOfWork.EmployeeRepo.GetAll().Where(e => e.User.IsDeleted == false).ToList();
           return _mapper.Map<List<ReadEmployeeDTO>>(employees);
        }
        public Employee? GetEmployeeById(int id)
        {
            var employee = _unitOfWork.EmployeeRepo.GetById(id);
            if (employee == null)
            {
                return null;
            }
            return employee;
        }
        public ReadEmployeeDTO? GetEmployeeDTOById(int id)
        {
            var employee = GetEmployeeById(id);
            return employee != null ?_mapper.Map<ReadEmployeeDTO>(employee): null;
        }
        public async Task<Employee> AddEmployee(AddEmployeeDTO addEmployeeDTO)
        {
            var employee = _mapper.Map<Employee>(addEmployeeDTO);

            var user = employee.User;

            
            await _userManager.CreateAsync(user , addEmployeeDTO.Password);

            employee.UserId = user.Id;

            await _unitOfWork.EmployeeRepo.AddAsync(employee);
            await _unitOfWork.SaveAsync();

            // add user role "Employee" by Default
            await _unitOfWork.UserManager.AddToRoleAsync(user, "Employee");
            // add the other specified role ex:("Admin", "HR" ..etc)
            await _unitOfWork.UserManager.AddToRoleAsync(user, addEmployeeDTO.SpecificRole);
            return employee;
        }
        public async Task UpdateEmployee(AddEmployeeDTO updateEmployeeDTO,Employee employee)
        {
            var user = employee.User;
            var passHashed = employee.User.PasswordHash;

            //_mapper.Map(updateEmployeeDTO, employee);
            employee.BranchId = updateEmployeeDTO.BranchId;
            //employee.SpecificRole = updateEmployeeDTO.SpecificRole;
            if (!string.IsNullOrWhiteSpace(updateEmployeeDTO.UserName) && user.UserName != updateEmployeeDTO.UserName)
            {
                var setUserNameResult = await _userManager.SetUserNameAsync(user, updateEmployeeDTO.UserName);
                if (!setUserNameResult.Succeeded)
                {
                    throw new ApplicationException(
                        $"Username update failure: {string.Join(", ", setUserNameResult.Errors.Select(e => e.Description))}");
                }
            }

            if (!string.IsNullOrWhiteSpace(updateEmployeeDTO.Email) && user.Email != updateEmployeeDTO.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, updateEmployeeDTO.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException(
                        $"Email update failure: {string.Join(", ", setEmailResult.Errors.Select(e => e.Description))}");
                }
            }
            user.FirstName = updateEmployeeDTO.FirstName;
            user.LastName = updateEmployeeDTO.LastName;
            user.PhoneNumber = updateEmployeeDTO.PhoneNumber;
            user.IsDeleted = updateEmployeeDTO.IsDeleted;
            
            if (updateEmployeeDTO.SpecificRole != employee.SpecificRole)
            {
                // remove the old role
                await _unitOfWork.UserManager.RemoveFromRoleAsync(user, employee.SpecificRole);
                // add the new role
                await _unitOfWork.UserManager.AddToRoleAsync(employee.User, updateEmployeeDTO.SpecificRole);
                employee.SpecificRole = updateEmployeeDTO.SpecificRole;
            }

            if (!string.IsNullOrEmpty(updateEmployeeDTO.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, updateEmployeeDTO.Password);
                if (!result.Succeeded)
                {
                    var newToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    result = await _userManager.ResetPasswordAsync(user, newToken, updateEmployeeDTO.Password);

                    if (!result.Succeeded)
                    {
                        throw new ApplicationException(
                           $"Permanent password update failure: {string.Join(", ", result.Errors.Select(e => e.Description))}");

                    }
                }
            }
            else
            {
                employee.User.PasswordHash = passHashed;
            }
            _unitOfWork.EmployeeRepo.Update(employee);
            await _unitOfWork.SaveAsync();
        }

        public void SoftDelete(Employee employee)
        {
            employee.User.IsDeleted = true;
            _unitOfWork.EmployeeRepo.Update(employee);
            _unitOfWork.Save();
        }
        public void HardDelete(Employee employee)
        {
            _unitOfWork.EmployeeRepo.Delete(employee);
            _unitOfWork.Save();
        }
    }
}