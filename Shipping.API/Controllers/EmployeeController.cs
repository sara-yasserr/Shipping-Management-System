using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.EmployeeDTOs;
using Shipping.BusinessLogicLayer.Helper;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet]
        public ActionResult<PagedResponse<ReadEmployeeDTO>> GetAll([FromQuery] PaginationDTO pagination)
        {
            var employeesDTO = employeeService.GetAllEmployees(pagination);
            if (employeesDTO == null)
            {
                return NotFound("No Employees Found");
            }
            return Ok(employeesDTO);
        }

        [HttpGet("without-pagination")]
        public ActionResult<List<ReadEmployeeDTO>> GetAllWithoutPagination()
        {
            var employeesDTO = employeeService.GetAllEmployeesWithoutPagination();
            if (employeesDTO == null)
            {
                return NotFound("No Employees Found");
            }
            return Ok(employeesDTO);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ReadEmployeeDTO> GetById(int id)
        {
            var employeeDTO = employeeService.GetEmployeeDTOById(id);
            if (employeeDTO == null)
            {
                return NotFound();
            }
            return Ok(employeeDTO);
        }

        [HttpPost]
        public async Task<ActionResult<AddEmployeeDTO>> Add([FromBody] AddEmployeeDTO addEmployeeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = await employeeService.AddEmployee(addEmployeeDTO);

            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, addEmployeeDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] AddEmployeeDTO updateEmployeeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            await employeeService.UpdateEmployee(updateEmployeeDTO,employee);
            return Ok();
        }

        [HttpDelete("SoftDelete/{id:int}")]
        public ActionResult SoftDelete(int id)
        {
            var employee = employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }
            employeeService.SoftDelete(employee);
            return NoContent();
        }

        [HttpDelete("HardDelete/{id:int}")]
        public ActionResult HardDelete(int id)
        {
            var employee = employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }
            employeeService.HardDelete(employee);
            return NoContent();
        }

    }
}
