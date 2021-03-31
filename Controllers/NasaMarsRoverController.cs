using DreamInMars.Client;
using DreamInMars.Dto;
using DreamInMars.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DreamInMars.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class NasaMarsRoverController : ControllerBase
    {
        private readonly INasaMarsRoverClient _nasaRoverClient;
        public NasaMarsRoverController(INasaMarsRoverClient nasaRoverService)=>
                _nasaRoverClient = nasaRoverService;

        [HttpGet]
        [Route("Photos")]
        public async Task<IActionResult> GetRoverPhotos(RoverEnums rover, int page, DateTime earthDate)
        {
            if (page < 0) page = 1;
            var result = await _nasaRoverClient.GetRoverImagesAsync(rover, page, earthDate);
            return result != null ? Ok(new Response(result)) : Ok(new Response(new List<PhotoPath>()));
        }
       
     }
}
