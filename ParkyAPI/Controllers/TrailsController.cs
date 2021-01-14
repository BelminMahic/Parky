using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/trails")]

    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecTrails")]

    public class TrailsController : Controller
    {
        private ITrailRepository TrailRepository;
        private readonly IMapper mapper;

        public TrailsController(ITrailRepository TrailRepository, IMapper mapper)
        {
            this.TrailRepository = TrailRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailsDto>))]

        public IActionResult GetTrails()
        {
            var objList = TrailRepository.GetTrails();

            var objDto = new List<TrailsDto>();

            foreach (var item in objList)
            {
                objDto.Add(mapper.Map<TrailsDto>(item));
            }

            return Ok(objDto);
        }
        [HttpGet("{id:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailsDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int id)
        {
            var Trail = TrailRepository.GetTrail(id);

            if (Trail == null)
                return NotFound();

            var natParkDto = mapper.Map<TrailsDto>(Trail);
            return Ok(natParkDto);
        }
        [HttpGet("GetTrailInNationalPark/{nationalParkId:int}")]
        [ProducesResponseType(200, Type = typeof(TrailsDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
            var TrailList = TrailRepository.GetTrailsInNationalPark(nationalParkId);

            if (TrailList == null)
                return NotFound();
            var objDto = new List<TrailsDto>();
            foreach (var item in TrailList)
            {
                objDto.Add(mapper.Map<TrailsDto>(item));

            }


            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailsDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public IActionResult CreateTrail([FromBody] TrailUpsertDto TrailDto)
        {
            if (TrailDto == null)
            {
                return BadRequest(ModelState);
            }

            if (TrailRepository.TrailExists(TrailDto.Name))
            {
                ModelState.AddModelError("", "Name of national park already exists!");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }

            var natParkObj = mapper.Map<Trail>(TrailDto);
            if (!TrailRepository.CreateTrail(natParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {natParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { id = natParkObj.Id }, natParkObj);
        }

        [HttpPatch("{id:int}", Name = "UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int id, [FromBody] TrailUpsertDto TrailDto)
        {
            if (TrailDto == null || id != TrailDto.Id)
            {
                return BadRequest(ModelState);
            }

            var natParkObj = mapper.Map<Trail>(TrailDto);
            if (!TrailRepository.UpdateTrail(natParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {natParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();


        }
        [HttpDelete("{id:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult DeleteTrail(int id)
        {
            if (!TrailRepository.TrailExists(id))
            {
                return NotFound();
            }

            var natParkObj = TrailRepository.GetTrail(id);
            if (!TrailRepository.DeleteTrail(natParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {natParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();


        }

    }
}
