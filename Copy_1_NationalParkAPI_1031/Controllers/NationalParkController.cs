using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Copy_1_NationalParkAPI_1031.Models;
using Copy_1_NationalParkAPI_1031.Models.DTOs;
using Copy_1_NationalParkAPI_1031.Repository.IRepository;

namespace Copy_1_NationalParkAPI_1031.Controllers
{
    [Route("api/NationalPark")]
    [ApiController]
    //[Authorize]
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;
        public NationalParkController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var nationalParkDto = _nationalParkRepository.GetNationalParks().
                Select(_mapper.Map<NationalPark, NationalParkDto>);
            return Ok(nationalParkDto);  // 200 

        }
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var nationalPark = _nationalParkRepository.GetNationalPark(nationalParkId);
            if (nationalPark == null) return NotFound(); //404
            var nationalDto = _mapper.Map<NationalParkDto>(nationalPark);
            return Ok(nationalDto);  // 200
        }
        [HttpPost]
        public IActionResult CreatNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest(); // 400
            if (_nationalParkRepository.NationalParkExists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park in DB !!!");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest(); // 400
            var nationlPark = _mapper.Map<NationalParkDto, NationalPark>(nationalParkDto);
            if (!_nationalParkRepository.CreateNationPark(nationlPark))
            {
                ModelState.AddModelError("", $"Something went wrong while save data : {nationlPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //return Ok(); // 200
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationlPark.Id },
                nationlPark);
        }
        [HttpPut]
        public IActionResult UpdateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null) return BadRequest(); // 400
            if (!ModelState.IsValid) return BadRequest(); // 400
            var nationalPark = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_nationalParkRepository.UpdateNationPark(nationalPark))
            {
                ModelState.AddModelError("", $"Somthing wewnt wrong while update data: {nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);  
            }
            return NoContent(); //204
        }
        [HttpDelete("{NationalParkId:int}")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_nationalParkRepository.NationalParkExists(nationalParkId)) return NotFound();
            var nationalPark = _nationalParkRepository.GetNationalPark(nationalParkId);
            if(nationalPark == null) return NotFound();
            if (_nationalParkRepository.DeleteNationPark(nationalPark))
            {
                ModelState.AddModelError("", $"Somthing went wrong while delete Data : {nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
