using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTBottle.Interfaces;
using RESTBottle.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// Dette er en controller, som håndterer HTTP requests til /api/bottles
// Exceptions håndteres i repository, så controlleren kun skal håndtere HTTP requests og responses
//TODO: kommenter JWT, inden python koden laves i vs code - Done kl 27/10 kl 09:40
namespace RESTBottle.Controllers
{
    [Route("api/[controller]")]//Dette er en route, som bestemmer hvordan man kan tilgå controller
    [ApiController]
    public class BottlesController : ControllerBase
    {
        private IBottlesRepository _repo;

        public BottlesController(IBottlesRepository repo)
        {
            this._repo = repo;
        }
        // GET: api/<BottlesController>
        [ProducesResponseType(StatusCodes.Status200OK)]//fortæller at denne metode returnerer en 200 OK status kode
        [ProducesResponseType(StatusCodes.Status204NoContent)]//fortæller at denne metode kan returnere en 204 No Content status kode
        [HttpGet]
        [Authorize(Roles = "User, Admin")]//kun brugere med rollen Admin eller User kan tilgå denne metode
        public ActionResult<IEnumerable<Bottle>> Get(
            [FromQuery] string? nameStartsWith = null, 
            [FromQuery] int? minimumVolume = null, 
            [FromQuery] string? sortOrder = null)//vi bruger repository til at hente alle flasker fra databasen
        {
            //TODO: lav 204 No content besked hvis der ikke returneres noget.
            var bottles = _repo.GetBottles(nameStartsWith, minimumVolume, sortOrder);//Brugeren skal skrive filteret i en quwryparameter, feks: /api/Bottles?nameStartsWith=B 
            if (bottles.Any())
            {
                return Ok(bottles);//returnerer en 200 OK status kode med flaskerne som body   
            }
            return NoContent();//returnerer en 204 No Content status kode
        }

        // GET api/<BottlesController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]//fortæller at denne metode returnerer en 200 OK status kode
        [ProducesResponseType(StatusCodes.Status404NotFound)]//fortæller at denne metode kan returnere en 404 Not Found status kode
        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]//kun brugere med rollen Admin eller User kan tilgå denne metode
        public ActionResult<Bottle?> Get([FromRoute] int id)
        {
            var bottle = _repo.GetById(id);
            if (bottle == null)
            {
                return NotFound("Bottle Not Found with id: " + id);
            }
            return Ok(bottle);
        }

        // POST api/<BottlesController>
        [ProducesResponseType(StatusCodes.Status201Created)]//fortæller at denne metode returnerer en 200 OK status kode
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<Bottle> Post([FromBody] Bottle value)
        {
            Bottle newBottle = _repo.AddBottle(value);
            return CreatedAtAction(nameof(Get), new { id = newBottle.Id, }, newBottle);
        }

        // PUT api/<BottlesController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]//fortæller at denne metode returnerer en 200 OK status kode
        [ProducesResponseType(StatusCodes.Status404NotFound)]//fortæller at denne metode kan returnere en 404 Not Found status kode
        public ActionResult<Bottle> Put(int id, [FromBody] Bottle value) //[FromBody] betyder at værdien kommer fra bodyen af requesten
        {
            Bottle bottle = _repo.Update(id, value);
            if (bottle == null)
            {
                return NotFound("No Bottle with id: " + id);

            }
            return Ok(bottle);
        }

        // DELETE api/<BottlesController>/5
        [ProducesResponseType(StatusCodes.Status200OK)]//fortæller at denne metode returnerer en 200 OK status kode
        [ProducesResponseType(StatusCodes.Status404NotFound)]//fortæller at denne metode kan returnere en 404 Not Found status kode
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult<Bottle> Delete(int id)
        {
            Bottle deletedBottle = _repo.DeleteById(id);
            if (deletedBottle == null)
            {
                return NotFound("No Bottle with id: " + id);
            }
            return Ok(deletedBottle);
        }
    }
}
