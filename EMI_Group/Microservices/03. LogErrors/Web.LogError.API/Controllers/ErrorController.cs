using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories;

namespace TransactionHistory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : Controller
    {
        private IErrorCollection db = new ErrorCollection();

        [HttpGet]
        public async Task<IActionResult> GetAllErrors()
        {
            return Ok(await db.GetAllErrors());
        }

        [HttpGet("FirstTenRecords")]
        public async Task<IActionResult> GetErrorDetailsFirstTenRecords()
        {
            return Ok(await db.GetAllErrors());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetErrorDetails(string id)
        {
            return Ok(await db.GetErrorById(id));
        }

        [HttpPost]
        public async Task<IActionResult> InsertError([FromBody] Error error)
        {
            if (error == null) return BadRequest();
            await db.InsertError(error);
            return Created("Created", true);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> updateError([FromBody] Error error, string id)
        {
            if (error == null) return BadRequest();
            error.Id = new MongoDB.Bson.ObjectId(id);
            await db.UpdateError(error);
            return Created("Created", true);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteError(string id)
        {
            await db.DeleteError(id);
            return NoContent();

        }
    }
}
