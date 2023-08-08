using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataService dataService;

        public DataController(IDataService dataService)
        {
            this.dataService = dataService;
        }
        // GET: api/<DataController>
        [HttpGet]
        public ActionResult<List<Data>> Get()
        {
            return dataService.Get();
        }

        // GET api/<DataController>/5
        [HttpGet("{id}")]
        public ActionResult<Data> Get(string id)
        {
            var data = dataService.Get(id);
            if (data == null)
            {
                return NotFound("id not found");
            }
            return data;
        }

        // POST api/<DataController>
        [HttpPost]
        public ActionResult<Data> Post([FromBody] Data data)
        {
            dataService.Create(data);
            return CreatedAtAction(nameof(Get), new { id = data.Id }, data);
        }

        // PUT api/<DataController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Data data)
        {
            var existingData = dataService.Get(id);
            if (existingData == null)
            {
                return NotFound("id not found");
            }
            dataService.Update(id, data);
            return NoContent();
        }

        // DELETE api/<DataController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var data = dataService.Get(id);
            if (data == null)
            {
                return NotFound("id not found");
            }
            dataService.Remove(data.Id);
            return Ok("entry deleted");
        }
    }
}
