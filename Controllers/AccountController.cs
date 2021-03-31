using AutoMapper;
using DreamInMars.Dto;
using DreamInMars.Model;
using DreamInMars.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DreamInMars.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase 
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IMapper _mapper;
        public AccountController(IAccountRepository repo, IMapper mapper) 
        {
            _accountRepo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAccount(int id) 
        {
            var result = await _accountRepo.ReadAsync(id);
            return result != null ? Ok(new Response(result)) : AccountNotFound();
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateAccount(AccountInfo accountInfo)
        {
            var account = _mapper.Map<Account>(accountInfo);
            var result = await _accountRepo.UpdateAsync(account);
            return result != null ? Ok(new Response(result)) : AccountNotFound();
        }

        private IActionResult AccountNotFound() => new NotFoundObjectResult(new Response("Account not found."));
    }
}