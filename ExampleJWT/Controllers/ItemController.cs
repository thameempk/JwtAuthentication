using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExampleJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {

        List<Items> items = new List<Items>
        {
            new Items{ Id = 1, Name = "lear redux"},
            new Items{ Id = 2, Name = "learn html"}
        };
       
        [Authorize]
        [HttpGet]
        public IActionResult GetItems()
        {
            return Ok(items);
        }
    }
}
