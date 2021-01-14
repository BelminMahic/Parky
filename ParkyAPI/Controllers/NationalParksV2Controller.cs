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
    [Route("api/v{version:apiVersion}/nationalparks")]
    [ApiVersion("2.0")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[ApiExplorerSettings(GroupName = "ParkyOpenAPISpecNP")]
    public class NationalParksV2Controller : Controller
    {
        private INationalParkRepository nationalParkRepository;
        private readonly IMapper mapper;

        public NationalParksV2Controller(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            this.nationalParkRepository = nationalParkRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(200,Type=typeof(List<NationalParkDto>))]

        public IActionResult GetNationalParks()
        {
            var obj = nationalParkRepository.GetNationalParks().FirstOrDefault();

            

            return Ok(mapper.Map<NationalParkDto>(obj));
        }
      
    }
}
