using AutoMapper;
using DreamInMars.Configuration;
using DreamInMars.Dto;
using DreamInMars.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace DreamInMars.Logic
{
    public class AuthenticationLogic : IAuthenticationLogic
    {
        private readonly UserManager<DreamUser> _userManager;
        private readonly IAccountLogic _accountLogic;
        private readonly AuthenticationConfiguration _authConfig;
        private readonly IMapper _mapper;
        const string PROVIDER = "Google";

        public AuthenticationLogic(
            UserManager<DreamUser> userManager, 
            IAccountLogic accountLogic,
            IOptions<AuthenticationConfiguration> config,
            IMapper mapper)
        {
            _userManager = userManager;
            _accountLogic = accountLogic;
            _authConfig = config.Value;
            _mapper = mapper;
        }

        public async Task<AuthenticatedToken> AuthenticateGoogleIdTokenAsync(string idToken)
        {
            var settings = new ValidationSettings { Audience = new[] { _authConfig.ClientId } };
            var payload = await ValidateAsync(idToken, settings);
            var account = await AddUserAsync(PROVIDER, payload);
            return GenerateUserToken(payload, account);
        }
        
        private AuthenticatedToken GenerateUserToken(Payload payload, AccountInfo account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authConfig.JwtSecret);
            var expires = DateTime.UtcNow.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, payload.Email) ,
                    new Claim(JwtRegisteredClaimNames.Sub, _authConfig.Subject),
                    new Claim(ClaimTypes.Surname, payload.FamilyName),
                    new Claim(ClaimTypes.GivenName, payload.GivenName),
                    new Claim(ClaimTypes.NameIdentifier, payload.Email),
                    new Claim(ClaimTypes.Email, payload.Email)
                }),

                Expires = expires,

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _authConfig.Issuer,
                Audience = _authConfig.Audience
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var token = tokenHandler.WriteToken(securityToken);

            return new AuthenticatedToken
            {
                Token = token,
                Expires = expires,
                IsAuthenticated = true,
                Account = account
            };
        }
     
        private async Task<AccountInfo> AddUserAsync(string provider, Payload payload)
        {
            IdentityResult identityResult = null;
            Account account = null;
            var user = await _userManager.FindByNameAsync(payload.Email);
          
            if (user == null)
            {
                account = new Account
                {
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    Avatar = payload.Picture
                };

                account.AccountId = await _accountLogic.CreateAccount(account);

                user = new DreamUser
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    AccountId = account.AccountId,
                    Id = payload.Subject
                };
                var info = new UserLoginInfo(provider, payload.Subject, provider.ToUpperInvariant());
                identityResult =  await _userManager.CreateAsync(user);
                identityResult = identityResult.Succeeded ? await _userManager.AddLoginAsync(user, info) : identityResult;
            }

           if ((identityResult?.Succeeded).GetValueOrDefault(true))
            {
                return _mapper.Map<AccountInfo>(account) ?? await _accountLogic.GetAccountAsync(user.AccountId);
            }
            throw new SystemException($"The following errors occurred:{string.Join(Environment.NewLine, identityResult.Errors)}");
        }
    }
}