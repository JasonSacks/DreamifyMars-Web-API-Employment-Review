using DreamInMars.Model;
using System.Threading.Tasks;

namespace DreamInMars.Logic
{
    public interface ICreditLogic
    {
        Task<Credit> GetCreditsAsync(int accountId);
    }
}