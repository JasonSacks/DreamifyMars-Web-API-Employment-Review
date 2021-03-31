using DreamInMars.Dto;
using DreamInMars.Model;
using System.Data;
using System.Threading.Tasks;

namespace DreamInMars.Repository
{
    public interface IAccountRepository
    {
        Task<int> CreateAsync(Account account, IDbConnection connection = null, IDbTransaction transaction = null);
        Task<AccountInfo> ReadAsync(int accountId);
        Task<AccountInfo> UpdateAsync(Account account);
    }
}