using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterApi.Models;
using ShelterApi.Models.DTOs;

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

        [HttpGet("animalById")]
        public async Task<IActionResult> getAnimalById([FromQuery]Guid id)
        {
            try
            {
                var animal = await _animalshelterContext.Animals.SingleOrDefaultAsync(ani=>ani.Id== id);

                if(animal == null)
                {
                    return StatusCode(404);
                }
                return StatusCode(200, animal);
            }
            catch (Exception e)
            {
                return StatusCode(404, new {message = e.Message});
            }
        }

        [HttpPost("newAnimal")]
        public async Task<IActionResult> newAnimal(NewAnimalDTO newAnimalDTO)
        {
            try
            {
                var shelterid = await _animalshelterContext.Shelters.AnyAsync(sh => sh.Id == newAnimalDTO.ShelterId);
                if (!shelterid)
                {
                    return StatusCode(404, new {message ="nincs ilyen menhely"});
                }

                var animal = new Animal
                {
                    Id = Guid.NewGuid(),
                    Name = newAnimalDTO.Name,
                    Species = newAnimalDTO.Species,
                    Age = newAnimalDTO.Age,
                    Weight = newAnimalDTO.Weight,
                    RegTime = DateTime.Now,
                    ShelterId = newAnimalDTO.ShelterId
                };

                if (animal != null)
                {
                    await _animalshelterContext.Animals.AddAsync(animal);
                    await _animalshelterContext.SaveChangesAsync();

                    return StatusCode(201, animal);
                }
                return StatusCode(404);
            }
            catch (Exception)
            {
                return StatusCode(404);
            }
            
        }

        [HttpDelete("deleteAnimal")]
        public async Task<IActionResult> deleteAnimal([FromQuery]Guid id)
        {
            try
            {
                var animal = await _animalshelterContext.Animals.SingleOrDefaultAsync(anim => anim.Id == id);

                if (animal != null)
                {
                    _animalshelterContext.Animals.Remove(animal);
                    await _animalshelterContext.SaveChangesAsync();

                    return StatusCode(200, new { message = "sikeres törlés!" });
                }
                return StatusCode(404);
            }
            catch (Exception e)
            {
                return StatusCode(404, new { message = e.Message });
            }
        }

        [HttpPut("updateAnimal")]
        public async Task<IActionResult> UpdateAnimal([FromQuery] Guid id, UpdateAnimalDTO updateAnimalDTO)
        {
            try
            {
                var shelterid = await _animalshelterContext.Shelters.AnyAsync(sh => sh.Id == updateAnimalDTO.ShelterId);
                if (!shelterid)
                {
                    return StatusCode(404, new { message = "nincs ilyen menhely" });
                }
                var animal = await _animalshelterContext.Animals.SingleOrDefaultAsync(ani => ani.Id == id);

                if (animal != null)
                {
                    animal.Name = updateAnimalDTO.Name;
                    animal.Species = updateAnimalDTO.Species;
                    animal.Age = updateAnimalDTO.Age;
                    animal.Weight = updateAnimalDTO.Weight;
                    animal.ShelterId = updateAnimalDTO.ShelterId;

                    _animalshelterContext.Animals.Update(animal);
                    await _animalshelterContext.SaveChangesAsync();

                    return StatusCode(200, new { message = "sikeres frissítés" });
                }
                return StatusCode(404);
            }
            catch (Exception e)
            {
                return StatusCode(404, new {message = e.Message});
            }
        }

        [HttpGet("allAnimalsWithShelter")]
        public async Task<IActionResult> AnimalsWithShelters()
        {
            try
            {
                var animalsWithShelter = _animalshelterContext.Animals.Join(_animalshelterContext.Shelters, ani => ani.ShelterId, sh => sh.Id, (ani, sh) => new { Allat_neve = ani.Name, Allat_faja = ani.Species, Menhely_neve = sh.Name });

                if (animalsWithShelter != null)
                {
                    return StatusCode(200, animalsWithShelter);
                }
                return StatusCode(404);
            }
            catch (Exception)
            {
                return StatusCode(404);
            }
        }
    }
}
