using DreamInMars.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace DreamInMars.Logic
{
    public interface IAuthenticationLogic
    {
        Task<AuthenticatedToken> AuthenticateGoogleIdTokenAsync(string idToken);
    }
}