using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShelterApi.Models;
using Microsoft.EntityFrameworkCore;
using ShelterApi.Models.DTOs;

namespace ShelterApi.Controllers
{
    [Route("/shelters")]
    [ApiController]
    public class SheltersController : ControllerBase
    {
        private readonly AnimalshelterContext _animalshelterContext;
        public SheltersController(AnimalshelterContext animalshelterContext)
        {
            _animalshelterContext = animalshelterContext;
        }

        [HttpGet("allShelter")]
        public async Task<IActionResult> allShelter()
        {
            try
            {
                var shelters = await _animalshelterContext.Shelters.ToListAsync();
                if(shelters != null)
                {
                    return StatusCode(200, shelters);
                }
                return StatusCode(404, new {message = "A tömb üres."});
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }  
        }

        [HttpGet("shelterbyId")]
        public async Task<IActionResult> getbyId([FromQuery]Guid id)
        {
            try
            {
                var shelter = await _animalshelterContext.Shelters.FirstOrDefaultAsync(s => s.Id == id);

                if (shelter != null)
                {
                    return StatusCode(200, shelter);
                }
                return StatusCode(404);
            }
            catch (Exception e)
            {
                return StatusCode(404, new { message = e.Message });
            }
        }

        [HttpPost("newShelter")]
        public async Task<IActionResult> newShelter(NewShelterDTO newShelterDTO)
        {
            try
            {
                var shelter = new Shelter
                {
                    Id = Guid.NewGuid(),
                    Name = newShelterDTO.Name,
                    City = newShelterDTO.City,
                    Capacity = newShelterDTO.Capacity,
                    RegTime = DateTime.Now
                };

                if (shelter != null)
                {
                    await _animalshelterContext.AddAsync(shelter);
                    await _animalshelterContext.SaveChangesAsync();

                    return StatusCode(201, shelter);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
    }
}
