using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Shipping.BusinessLogicLayer.DTOs.EmployeeDTOs;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;

namespace Shipping.BusinessLogicLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EmployeeService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<ReadEmployeeDTO> GetAllEmployees()
        {
            var employees = _unitOfWork.EmployeeRepo.GetAll();
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

            await _unitOfWork.EmployeeRepo.AddAsync(employee);
            await _unitOfWork.SaveAsync();

            // add user role "Employee" by Default
            await _unitOfWork.UserManager.AddToRoleAsync(employee.User, "Employee");
            // add the other specified role ex:("Admin", "HR" ..etc)
            await _unitOfWork.UserManager.AddToRoleAsync(employee.User, addEmployeeDTO.SpecificRole);
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