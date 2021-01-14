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
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    public class NationalParksController : Controller
    {
        private INationalParkRepository nationalParkRepository;
        private readonly IMapper mapper;

        public NationalParksController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            this.nationalParkRepository = nationalParkRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200,Type=typeof(List<NationalParkDto>))]

        public IActionResult GetNationalParks()
        {
            var objList = nationalParkRepository.GetNationalParks();

            var objDto = new List<NationalParkDto>();

            foreach (var item in objList)
            {
                objDto.Add(mapper.Map<NationalParkDto>(item));
            }

            return Ok(objDto);
        }
        [HttpGet("{id:int}",Name ="GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int id)
        {
            var nationalPark = nationalParkRepository.GetNationalPark(id);

            if (nationalPark == null)
                return NotFound();

            var natParkDto = mapper.Map<NationalParkDto>(nationalPark);
            return Ok(natParkDto);
        }
       

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if(nationalParkDto==null)
            {
                return BadRequest(ModelState);
            }

            if(nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "Name of national park already exists!");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            { return BadRequest(ModelState); }

            var natParkObj = mapper.Map<NationalPark>(nationalParkDto);
            if(!nationalParkRepository.CreateNationalPark(natParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {natParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetNationalPark",new {version=HttpContext.GetRequestedApiVersion().ToString(), id=natParkObj.Id },natParkObj);
        }

        [HttpPatch("{id:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int id, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || id!=nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }

            var natParkObj = mapper.Map<NationalPark>(nationalParkDto);
            if (!nationalParkRepository.UpdateNationalPark(natParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {natParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();


        }
        [HttpDelete("{id:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public IActionResult DeleteNationalPark(int id)
        {
            if (!nationalParkRepository.NationalParkExists(id))
            {
                return NotFound();
            }

            var natParkObj = nationalParkRepository.GetNationalPark(id);
            if (!nationalParkRepository.DeleteNationalPark(natParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {natParkObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();


        }
    }
}
