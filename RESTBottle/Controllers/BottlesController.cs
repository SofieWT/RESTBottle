using Microsoft.AspNetCore.Mvc;
using RESTBottle.Interfaces;
using RESTBottle.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        [HttpGet]
        public IEnumerable<Bottle> Get()//vi bruger repository til at hente alle flasker fra databasen
        {
            return _repo.GetBottles();
        }

        // GET api/<BottlesController>/5
        [HttpGet("{id}")]
        public Bottle? Get(int id)
        {
            //TODO handle null case, if bottle with id does not exist
            return _repo.GetById(id);
        }

        // POST api/<BottlesController>
        [HttpPost]
        public void Post([FromBody] Bottle value)
        {
            _repo.AddBottle(value);
        }

        // PUT api/<BottlesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Bottle value)
        {
            _repo.Update(id, value);
        }

        // DELETE api/<BottlesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _repo.DeleteById(id);
        }
    }
}
