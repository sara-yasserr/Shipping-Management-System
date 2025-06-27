using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs.EmployeeDTOs;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public EmployeeController(UnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        [HttpGet]
        public ActionResult<List<ReadEmployeeDTO>> GetAll()
        {
            var employees = unitOfWork.EmployeeRepo.GetAll();
            if (employees == null)
            {
                return NotFound("No Employees Found");
            }
            return Ok(mapper.Map<List<ReadEmployeeDTO>>(employees));
        }
        [HttpGet("{id:int}")]
        public ActionResult<ReadEmployeeDTO> GetById(int id)
        {
            var employee = unitOfWork.EmployeeRepo.GetById(id);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }
            return Ok(mapper.Map<ReadEmployeeDTO>(employee));
        }
        [HttpPost]
        public async Task<ActionResult<ReadEmployeeDTO>> Add([FromBody] AddEmployeeDTO addEmployeeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = mapper.Map<Employee>(addEmployeeDTO);
          
            await unitOfWork.EmployeeRepo.AddAsync(employee);
            await unitOfWork.SaveAsync();

            // add user role "Employee" by Default
            await unitOfWork.UserManager.AddToRoleAsync(employee.User, "Employee");
            // add the other specified role ex:("Admin", "HR" ..etc)
            await unitOfWork.UserManager.AddToRoleAsync(employee.User, addEmployeeDTO.Role);

            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, mapper.Map<ReadEmployeeDTO>(employee));
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ReadEmployeeDTO>> Update(int id, [FromBody] AddEmployeeDTO updateEmployeeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = unitOfWork.EmployeeRepo.GetById(id);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }
            mapper.Map(updateEmployeeDTO, employee);
            if(updateEmployeeDTO.Role != employee.User.Role)
            {
                // remove the old role
                await unitOfWork.UserManager.RemoveFromRoleAsync(employee.User, employee.User.Role);
                // add the new role
                await unitOfWork.UserManager.AddToRoleAsync(employee.User, updateEmployeeDTO.Role);
            }
            unitOfWork.EmployeeRepo.Update(employee);
            await unitOfWork.SaveAsync();
            return Ok(mapper.Map<ReadEmployeeDTO>(employee));
        }
        [HttpDelete("SmoothDelete/{id:int}")]
        public ActionResult Delete(int id)
        {
            var employee = unitOfWork.EmployeeRepo.GetById(id);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }
            employee.User.IsDeleted = true;
            unitOfWork.EmployeeRepo.Update(employee);
            unitOfWork.Save();
            return NoContent();
        }
        [HttpDelete("HardDelete/{id:int}")]
        public ActionResult HardDelete(int id)
        {
            var employee = unitOfWork.EmployeeRepo.GetById(id);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }
            unitOfWork.EmployeeRepo.Delete(employee);
            unitOfWork.Save();
            return NoContent();
        }

    }
}
