using DreamInMars.Model;
using System.Data;
using System.Threading.Tasks;

namespace DreamInMars.Repository
{
    public interface ICreditRepository
    {
        Task<int> CreateAsync(Credit credit, IDbConnection connection  = null, IDbTransaction transaction = null);
        Task<Credit> ReadAsync(int accountId);
        Task UpdateAsync(Credit credit, IDbConnection connection = null, IDbTransaction transaction = null);
    }
}