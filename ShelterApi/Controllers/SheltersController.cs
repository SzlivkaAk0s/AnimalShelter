using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShelterApi.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}
