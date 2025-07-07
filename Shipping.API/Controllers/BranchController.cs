using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.DTOs;
using Shipping.BusinessLogicLayer.DTOs.BranchDTOs;
using Shipping.BusinessLogicLayer.Helper;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.DataAccessLayer.Models;
using Shipping.DataAccessLayer.UnitOfWorks;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService branchService;
        public BranchController(IBranchService branchService)
        {
            this.branchService = branchService;
        }

        [HttpGet("paginated")]
        public ActionResult<PagedResponse<ReadBranch>> GetAll([FromQuery] PaginationDTO pagination)
        {
            var result = branchService.GetAllBranch(pagination);
            return Ok(result);
        }
        [HttpGet]
        public ActionResult<List<ReadBranch>> GetAll()
        {
            var result = branchService.GetAllBranch();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public ActionResult<Branch> GetById(int id)
        {
            var branchDTO = branchService.GetBranchDTOById(id);
            if (branchDTO == null)
            {
                return NotFound();
            }
            return Ok(branchDTO);
        }

        [HttpPost]
        public ActionResult<AddBranch> Post(AddBranch branchDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var branch = branchService.AddBranch(branchDTO);
            return CreatedAtAction(nameof(GetById), new { id = branch.Id }, branchDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ReadBranch> Put(int id, AddBranch branchDTO)
        {
            var branch = branchService.GetBranchById(id); ;
            if (branch == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            branchService.UpdateBranch(branchDTO, branch);
            return Ok();
        }
        [HttpDelete("Soft/{id:int}")]
        public ActionResult SoftDelete(int id)
        {
            var branch = branchService.GetBranchById(id);
            if (branch == null)
            {
                return NotFound();
            }
            branchService.SoftDelete(branch);
            return NoContent();
        }
        [HttpDelete("Hard/{id:int}")]
        public ActionResult HardDelete(int id)
        {
            var branch = branchService.GetBranchById(id);
            if (branch == null)
            {
                return NotFound();
            }
            branchService.HardDelete(branch);
            return NoContent();
        }

        [HttpPut("Activate/{id:int}")]
        public ActionResult Activate(int id)
        {
            branchService.ActivateBranch(id);
            return Ok();
        }
    }
}
