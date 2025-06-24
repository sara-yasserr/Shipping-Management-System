using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs.BranchDTOs;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public BranchController(UnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        [HttpGet]
        public List<Branch> GetAll()
        {
            return unitOfWork.BranchRepo.GetAll();
        }
        [HttpGet("{id:int}")]
        public ActionResult<Branch> GetById(int id)
        {
            var branch = unitOfWork.BranchRepo.GetById(id);
            if (branch == null)
            {
                return NotFound();
            }
            var branchDTO = mapper.Map<ReadBranch>(branch);
            return Ok(branch);
        }
        [HttpPost]
        public ActionResult<ReadBranch> Post(AddBranch branchDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var branch = mapper.Map<Branch>(branchDTO);
            return CreatedAtAction(nameof(GetById), new { id = branch.Id }, mapper.Map<ReadBranch>(branch));
        }
        [HttpPut("{id:int}")]
        public ActionResult<ReadBranch> Put(int id, AddBranch branchDTO)
        {
            var branch = unitOfWork.BranchRepo.GetById(id);
            if (branch == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            mapper.Map(branchDTO, branch);
            unitOfWork.BranchRepo.Update(branch);
            unitOfWork.Save();
            return Ok(mapper.Map<ReadBranch>(branch));
        }
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var branch = unitOfWork.BranchRepo.GetById(id);
            if (branch == null)
            {
                return NotFound();
            }
            unitOfWork.BranchRepo.Delete(branch);
            unitOfWork.Save();
            return NoContent();
        }
    }
}
