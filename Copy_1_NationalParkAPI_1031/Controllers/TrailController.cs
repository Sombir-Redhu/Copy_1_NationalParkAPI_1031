using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Copy_1_NationalParkAPI_1031.Models;
using Copy_1_NationalParkAPI_1031.Models.DTOs;
using Copy_1_NationalParkAPI_1031.Repository;
using Copy_1_NationalParkAPI_1031.Repository.IRepository;
using System.Diagnostics;

namespace Copy_1_NationalParkAPI_1031.Controllers
{
    [Route("api/Trail")]
    [ApiController]
    //[Authorize]

    public class TrailController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;
        public TrailController(ITrailRepository trailRepository, IMapper mapper)
        {
            _mapper = mapper;
            _trailRepository = trailRepository;
        }
        [HttpGet]
        public IActionResult GetTrails()
        {
            return Ok(_trailRepository.GetTrails().Select(_mapper.Map<Trail, TrailDto>));
        }
        [HttpGet("{trailId:int}" ,Name ="GetTrail")]
        public IActionResult GetTrail(int trailId)
        {
            var trail = _trailRepository.GetTrail(trailId);
            if (trail == null) return NotFound();
            var trailDto = _mapper.Map<TrailDto>(trail);
            return Ok(trailDto);
        }
        [HttpPost]
        public IActionResult CreateTrail([FromBody] TrailDto trailDto)
        {
            if(trailDto == null) return BadRequest();
            if(_trailRepository.TrailExists(trailDto.Id))
            {
                ModelState.AddModelError("", "Trail in Db !!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if(!ModelState.IsValid) return BadRequest();
            var trail = _mapper.Map<TrailDto, Trail>(trailDto);
            if (!_trailRepository.CreateTrail(trail))
            {
                ModelState.AddModelError("", $"Somthing went Wrong while Save Data :{trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetTrail", new {trailId=trail.Id},trail);
        }
        [HttpPut]
        public IActionResult UpdateTrail([FromBody] TrailDto trailDto)
        {
            if(trailDto == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var trail = _mapper.Map<Trail>(trailDto);
            if(!_trailRepository.UpdateTrail(trail))
            {
                ModelState.AddModelError("", $"Somthing went wrong while Update data : {trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
        [HttpDelete("{trailId:int}")]


        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trailRepository.TrailExists(trailId)) return NotFound();
            var trail = _trailRepository.GetTrail(trailId);
            if (trail == null) return NotFound();
            if (!_trailRepository.DeleteTrail(trail))
            {
                ModelState.AddModelError("", $"Somthing went wrong while delete data : {trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

    }
}
