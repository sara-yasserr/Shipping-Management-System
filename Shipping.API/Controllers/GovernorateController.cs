using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.BusinessLogicLayer.DTOs.GovernorateDTOs;
using Microsoft.AspNetCore.Authorization;

namespace Shipping.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernorateController : ControllerBase
    {
        private readonly IGovernorateService _governorateService;

        public GovernorateController(IGovernorateService governorateService)
        {
            _governorateService = governorateService;
        }

        // Get all gov active and not active 
        [HttpGet]
        //   GET /api/Governorate
        public IActionResult GetAll()
        {
            return Ok(_governorateService.GetAll());
        }

        //get gov by id
        //   GET /api/Governorate/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _governorateService.GetById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }


        //add gov
        //  POST /api/Governorate
        [HttpPost]
        public IActionResult Add(AddGovernorateDto dto)
        {
            var success = _governorateService.AddGovernorate(dto);
            if (!success)
                return BadRequest();
            return Ok();
        }


        //Edit gov
        // PUT /api/Governorate/{id}
        [HttpPut("{id}")]
        public IActionResult Edit(int id, AddGovernorateDto dto)
        {
            var success = _governorateService.EditGovernorate(id, dto);
            if (!success)
                return NotFound();
            return Ok();
        }


        //soft delete
        [HttpDelete("SoftDelete/{id}")]
        //api/Governorate/SoftDelete/5
        public IActionResult SoftDelete( int id)
        {
            var success = _governorateService.SoftDeleteGovernorate(id);
            if (!success)
                return NotFound();
            return Ok();
        }


        //hard delete
        //api/Governorate/HardDelete/5
        [Authorize(Roles = "Admin")] //delete from DB only if he is adminnn
        [HttpDelete("HardDelete/{id}")]
        public IActionResult HardDeleteGovernorate(int id)
        {
            var success = _governorateService.HardDeleteGovernorate(id);
            if (!success)
                return NotFound();
            return Ok();
        }
   
 
    }
}