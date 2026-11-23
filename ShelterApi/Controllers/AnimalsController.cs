using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterApi.Models;

namespace ShelterApi.Controllers
{
    [Route("/animals")]
    [ApiController]
    public class AnimalsController : ControllerBase
    {
        private readonly AnimalshelterContext _animalshelterContext;
        public AnimalsController(AnimalshelterContext animalshelterContext)
        {
            _animalshelterContext = animalshelterContext;
        }

        [HttpGet("allAnimal")]
        public async Task<IActionResult> getAllAnimal()
        {
            try
            {
                var animals = await _animalshelterContext.Animals.ToListAsync();

                if (animals != null)
                {
                    return StatusCode(200, animals);
                }
                return StatusCode(404);
            }
            catch (Exception e)
            {
                return StatusCode(404, new { message = e.Message });
            }
        }
    }
}
