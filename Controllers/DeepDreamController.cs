using DreamInMars.Configuration;
using DreamInMars.Dto;
using DreamInMars.Exceptions;
using DreamInMars.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DreamInMars.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class DeepDreamController : ControllerBase
    {
        private readonly IDeepDreamImageLogic _logic;
        private readonly CreditConfiguration _creditConfiguration;

        public DeepDreamController(IDeepDreamImageLogic logic,IOptions<CreditConfiguration> creditConfig)
        {
            _logic = logic;
            _creditConfiguration = creditConfig.Value;
        }

        [HttpPost("TransformImage")]
        public async Task<IActionResult> SaveImage(Image image) 
        {
            try 
            { 
                var path = await _logic.TransformAndSaveImageAsync(image, _creditConfiguration.PaymentCredit);
                return Ok(new Response(new { path }));
            }
            catch(OutOfCreditsException)
            {
                return Ok(new Response("Sorry you are out of credits. Come back again tomorrow!"));
            }
        }
    }
}
