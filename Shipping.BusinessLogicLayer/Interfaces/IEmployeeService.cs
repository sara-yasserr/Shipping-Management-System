using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.EmployeeDTOs;
using Shipping.BusinessLogicLayer.Helper;
using Shipping.DataAccessLayer.Models;

namespace Shipping.BusinessLogicLayer.Interfaces
{
    public interface IEmployeeService
    {
        public PagedResponse<ReadEmployeeDTO> GetAllEmployees(PaginationDTO pagination);
        public List<ReadEmployeeDTO> GetAllEmployee();
        public Employee? GetEmployeeById(int id);
        public ReadEmployeeDTO? GetEmployeeDTOById(int id);
        public Task<Employee> AddEmployee(AddEmployeeDTO addEmployeeDTO);
        public Task UpdateEmployee(AddEmployeeDTO updateEmployeeDTO, Employee employee);
        public void SoftDelete(Employee employee);
        public void HardDelete(Employee employee);

    }
}
