using DreamInMars.Dto;
using DreamInMars.Model;
using System.Threading.Tasks;

namespace DreamInMars.Logic
{
    public interface IAccountLogic
    {
        Task<AccountInfo> GetAccountAsync(int accountId);
        Task<int> CreateAccount(Account account);
    }
}