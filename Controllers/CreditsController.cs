using DreamInMars.Dto;
using DreamInMars.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DreamInMars.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CreditsController : ControllerBase
    {
        private readonly ICreditLogic _creditLogic;

        public CreditsController(ICreditLogic logic)  =>  
            _creditLogic = logic;

        [HttpGet]
        [Route("{accountId}")]
        public async Task<IActionResult> GetCredits(int accountId)
        {
            var response = new Response(await _creditLogic.GetCreditsAsync(accountId));
            return response != null ? Ok(response) : NotFound(new Response("credits not found"));
        }
    }
}
