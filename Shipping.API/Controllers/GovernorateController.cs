using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shipping.BusinessLogicLayer.Interfaces;
using Shipping.BusinessLogicLayer.DTOs.GovernorateDTOs;

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

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_governorateService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _governorateService.GetById(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Add(AddGovernorateDto dto)
        {
            var success = _governorateService.AddGovernorate(dto);
            if (!success)
                return BadRequest();
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, AddGovernorateDto dto)
        {
            var success = _governorateService.EditGovernorate(id, dto);
            if (!success)
                return NotFound();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _governorateService.DeleteGovernorate(id);
            if (!success)
                return NotFound();
            return Ok();
        }
    }
}
