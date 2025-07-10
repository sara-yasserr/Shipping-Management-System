using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
            var employees = _unitOfWork.EmployeeRepo.GetAll().Where(e => e.User.IsDeleted == false);
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
            _mapper.Map(updateEmployeeDTO, employee);
            if (updateEmployeeDTO.SpecificRole != employee.SpecificRole)
            {
                // remove the old role
                await _unitOfWork.UserManager.RemoveFromRoleAsync(employee.User, employee.SpecificRole);
                // add the new role
                await _unitOfWork.UserManager.AddToRoleAsync(employee.User, updateEmployeeDTO.SpecificRole);
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