using DreamInMars.Dto;
using DreamInMars.Logic;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DreamInMars.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GoogleController : ControllerBase
    {
        private readonly IAuthenticationLogic _authenticationLogic;

        public GoogleController(IAuthenticationLogic authenticationLogic)  =>
            _authenticationLogic = authenticationLogic;

        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate(AuthorizationRequest request) 
        {
            if (string.IsNullOrWhiteSpace(request?.TokenId))
                return BadRequest("IdToken is Requiered");
            try
            {
                var token = await _authenticationLogic.AuthenticateGoogleIdTokenAsync(request.TokenId);
                return Ok(new Response(token));
            }
            catch (InvalidJwtException)
            {
                return Unauthorized(new Response("Unauthorized: please log in"));
            }
        }
    }
}
